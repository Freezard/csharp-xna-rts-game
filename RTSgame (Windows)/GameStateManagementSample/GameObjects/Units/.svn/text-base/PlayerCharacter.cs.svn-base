using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects.Abstract;
using RTSgame.Utilities;
using RTSgame.AI;
using RTSgame.GameObjects.Buildings;
using RTSgame.Utilities.Graphics.ParticleSystem;
using RTSgame.GameObjects.Projectiles;
using RTSgame.Utilities.IO;
using SkinnedModel;
using RTSgame.GameObjects.Economy.Buildings;
using RTSgame.GameObjects.Economy;
using RTSgame.Utilities.Game;
using RTSgame.GameObjects.Components;
using RTSgame.Utilities.IO.UI;



namespace RTSgame.GameObjects.Units
{
    // The player character

    class PlayerCharacter : Unit, IInputtable, IMovable, ILogic, IAnimated
    {

        Vector2 velocity = Vector2.Zero;

        private const float SPEED = 8.0f;
        private bool buildMenu = false, addFollower = false, removeFollower = false, useBuilding = false, changeFormation = false;
        static private Surroundings playerSurroundings;
       // private Player Owner;
       
        private float heightModifier = 0;
        private Boolean charging;
        private int shotCharge;

        private BuildingNode closestBuildingNode; float closestBuildingNodeDist = float.PositiveInfinity;
        private Minion closestMinionNotInGroup; float closestMinionNotInGroupDist = float.PositiveInfinity;
        private IUsableStructure closestUsableBuilding; float closestUsableBuildingDist = float.PositiveInfinity;
        private GameObject autoAimTarget; float autoAimTargetAngle = float.PositiveInfinity;
        private float bobAmount = 0;
        Group minionGroup = new Group();
        private const float MAX_CONTROL_DISTANCE = 5*5;


        private AnimationPlayer animationPlayer;
        private AnimationPlayer secondPlayer;

        private Vector2 tiltAngle;
        private float rotation;

        public PlayerCharacter(Vector2 pos, Player player)
            : base(pos, new LayeredSkinnedModelComponent("PlayerCharacterHover", player.GetPlayerColor()), player, 100)
        {
            scale = 0.1f;

           // texture = player.GetPlayerColor();
            animationPlayer = ((SkinnedModelComponent)ModelComp).AnimationPlayer;
            secondPlayer = ((LayeredSkinnedModelComponent)ModelComp).SecondPlayer;
            //animationPlayer.StartClip("Take 001");
           // secondPlayer.UseBindPose("Take 001");

            secondPlayer.StartClip("Take 001");
            animationPlayer.UseBindPose("Take 001");

          //  idle = this.getClip("Take 001");
           // firing = this.getClip("Fire");
          //  special = this.getClip("SpecialMove2");

            
            player.SetControlledUnit(this);
           // animationPlayer.SetIdleClip(idle, true);
           // animationPlayer.UseBindPose(idle);
            //secondaryAnimationPlayer.StartClip(idle);
            //animationPlayer.StartClip(idle);
            InitializeCollisionBox();
            
            
        }
        
        
        public void HandleInput()
        {



            InputManager input = Owner.UpdateAndGetInput();
            Vector2 controlVector = input.LeftTSVector;
            if (controlVector.LengthSquared() > 1)
            {
                controlVector = Vector2.Normalize(controlVector);
            }
            velocity = controlVector * SPEED;
            buildMenu = input.ShoulderLeft || input.ShoulderRight;

            Boolean specialMovePressed = input.LeftTriggerPressed;
            float shootAmount = input.RightTriggerAmount;
            Boolean shootPressed = input.RightTriggerPressed;
            Boolean shootDown = input.RightTriggerDown;


            if (!Owner.BuildMenuOpen())
            {
                //Interpret buttons as "normal" ie no build menu open
                addFollower = input.ButtonA;
                removeFollower = input.ButtonB;
                useBuilding = input.ButtonX;
                changeFormation = input.ButtonY;

                //Show which building will be used when pressing "Use"
                if (closestUsableBuilding != null)
                {
                    Owner.GetUI().SetHighLight(closestUsableBuilding);
                }
                else
                {
                    Owner.GetUI().ClearHighLight();
                }

                if (addFollower)
                {
                    //miniongroup add closestminion that is controlled by this player
                    if (closestMinionNotInGroup != null)
                    {
                        minionGroup.Add(closestMinionNotInGroup);
                        SoundPlayer.PlaySound("addminion");
                    }
                }
                if (removeFollower)
                {
                    if (!minionGroup.IsEmpty())
                    {
                        SoundPlayer.PlaySound("deny");
                        minionGroup.RemoveClosestMinionInGroup(this.GetPosition());
                    }
                    
                }
                if (useBuilding)
                {

                    if (closestUsableBuilding != null)
                    {
                        closestUsableBuilding.AttemptToUse(this);
                    }
                }
                if (changeFormation)
                {
                    SoundPlayer.PlaySound("changeformation");
                    minionGroup.SwitchFormation();
                    //Owner.GetUI().AddUIComponent(new Floater(new TextComponent(Vector2.Zero, "u r mr gay", "gamefont", Color.Red), Owner.GetUI(), this.GetPositionV3(), false));
                    //Owner.GetUI().AddUIComponent(new Floater(new UnitHealthBar(Vector2.Zero, "UIBar", Color.Red, UIBar.Direction.Left, false, this, new Vector2(20,20)), Owner.GetUI(), this.GetPositionV3(), false));
                }
            }
            else
            {
                //Buttons correspond to creating buildings
                if (closestBuildingNode != null)
                {
                    Boolean buildWindmill = input.ButtonX;
                    Boolean buildFactory = input.ButtonA;
                    Boolean buildTower = input.ButtonY;
                    Boolean buildShrine = input.ButtonB;
                    if (buildShrine)
                    {
                        if (Owner.HasMetalWithSignal(Constants.DESIGN_SHRINE_COST))
                        {
                            SoundPlayer.PlaySound("build");
                            Owner.SubtractMetal(Constants.DESIGN_SHRINE_COST);
                            HealingBuilding healingb = new HealingBuilding(closestBuildingNode, false, Owner);
                            GameState.GetInstance().addGameObject(healingb);
                            DrawManager.GetInstance().addDrawable(healingb);
                            Owner.GetUI().SetBuildMenuOpen(!Owner.BuildMenuOpen());
                        }
                        

                    }
                    if (buildWindmill)
                    {
                        if (Owner.HasMetalWithSignal(Constants.DESIGN_WINDMILL_COST))
                            {
                                SoundPlayer.PlaySound("build");
                                Owner.SubtractMetal(Constants.DESIGN_WINDMILL_COST);
                                Windmill windmill = new Windmill(closestBuildingNode, false, Owner);
                                GameState.GetInstance().addGameObject(windmill);
                                DrawManager.GetInstance().addDrawable(windmill);
                                Owner.GetUI().SetBuildMenuOpen(!Owner.BuildMenuOpen());
                            }
                    }

                    if (buildFactory)
                    {
                        if(Owner.HasMetalWithSignal(Constants.DESIGN_FACTORY_COST)){
                            SoundPlayer.PlaySound("build");
                            Owner.SubtractMetal(Constants.DESIGN_FACTORY_COST);
                            RobotFactory factory = new RobotFactory(closestBuildingNode, false, Owner);
                        GameState.GetInstance().addGameObject(factory);
                        DrawManager.GetInstance().addDrawable(factory);
                        Owner.GetUI().SetBuildMenuOpen(!Owner.BuildMenuOpen());
                            }
                        
                    }
                    if (buildTower)
                    {
                        if (Owner.HasMetalWithSignal(Constants.DESIGN_PROTECT_BUILDING_COST))
                        {
                            SoundPlayer.PlaySound("build");
                            Owner.SubtractMetal(Constants.DESIGN_PROTECT_BUILDING_COST);
                            ProtectBuilding protectb = new ProtectBuilding(closestBuildingNode, false, Owner);
                            GameState.GetInstance().addGameObject(protectb);
                            DrawManager.GetInstance().addDrawable(protectb);
                            Owner.GetUI().SetBuildMenuOpen(!Owner.BuildMenuOpen());
                        }

                    }

                //Update UI to show where to construct building
                    Owner.GetUI().SetHighLight(closestBuildingNode);
                }
                else
                {
                    Owner.GetUI().ClearHighLight();
                }

                

            }



            //Toggle build menu
            if (buildMenu)
            {
                SoundPlayer.PlaySound("buildmenu");
                    Owner.GetUI().SetBuildMenuOpen(!Owner.BuildMenuOpen());
            }
            




            if (specialMovePressed)
            {
                //Constants.TweakVector.Y++;
                this.Damage(40);
               
                RumbleManager.StartVibration(Owner.GetPlayerIndex(), 1.0f, 1.0f, 500);

                animationPlayer.StartClip("SpecialMove2", false, 1.0f);
                //animationPlayer.StartClip(special, false,1.0f);
                SoundPlayer.Play3DSound("special", this.GetPositionV3());
                for (int i = 0; i < 8; i++)
                {
                    Projectile newShot = //new SimpleShot<Enemy>(this, Calculations.AngleToGameV2(GetFacingAngleOnXZPlane() + i * 40 - Calculations.Pi2th, 1), Vector3.Zero);
                        new PlayerCodedProjectile(this, Owner, Calculations.AngleToGameV2(GetFacingAngleOnXZPlane() + i * 40 - Calculations.Pi2th, 1), Vector3.Zero, 10); 
                        GameState.GetInstance().addGameObject(newShot);
                    //DrawManager.GetInstance().addDrawable(newShot);
                }
		        ParticleManager.GetInstance().AddSpecial(GetPositionV3() + new Vector3(0, 1, 0), Vector3.Zero, 0.7f);

                
            }

            if (shootPressed && !charging)
            {
                charging = true;
                shotCharge = 0;
                
            }
            if (charging)
            {
                int chargeAmount = (int)(shootAmount * Constants.DESIGN_PLAYER_DISCHARGE_RATE);
                if (Owner.HasEnergy(chargeAmount))
                {
                    shotCharge += chargeAmount;
                    Owner.SubtractEnergy(chargeAmount);
                }

                if (!shootDown || !Owner.HasEnergy(chargeAmount))
                {
                    animationPlayer.StartClip("Fire", false, 3.5f);
                    //animationPlayer.SwitchClipWithBlend(firing, 500, 0.1f, false, 2.0f);
                    SoundPlayer.Play3DSound("shoot", this.GetPositionV3());
                    RumbleManager.StartVibration(Owner.GetPlayerIndex(), 0.2f, 0.2f, 250);
                    const float projectileSpawnHeight = 0.25f;
                    const float projectileSpawnAheadOfPlayer = 0.65f;
                    Projectile newShot;
                    if (autoAimTarget == null)
                    {
                       newShot = new PlayerCodedProjectile(
                            this,
                            Owner,
                            Calculations.AngleToGameV2(GetFacingAngleOnXZPlane() - Calculations.Pi2th, 1),
                            Calculations.V2ToV3(Calculations.AngleToGameV2(GetFacingAngleOnXZPlane() - Calculations.Pi2th, projectileSpawnAheadOfPlayer), projectileSpawnHeight),
                            shotCharge);
                    }
                    else
                    {
                        newShot = new PlayerCodedProjectile(
                            this,
                            Owner,
                            autoAimTarget,
                            true,
                            Calculations.V2ToV3(Calculations.AngleToGameV2(GetFacingAngleOnXZPlane() - Calculations.Pi2th, projectileSpawnAheadOfPlayer), projectileSpawnHeight),
                            shotCharge);
                    }
                    GameState.GetInstance().addGameObject(newShot);
                    //DrawManager.GetInstance().addDrawable(newShot);

                    //Console.WriteLine(shotCharge);//TODO add damage to shot
                    charging = false;
                }
            }      




            //Reset!
           // Console.WriteLine("closestBuilding: " + closestUsableBuilding);
            //Console.WriteLine("closestBuildingNode: " + closestBuildingNode);
          //  Console.WriteLine("closestMin : " + closestMinionNotInGroup);
           
            closestBuildingNode = null; closestBuildingNodeDist = float.PositiveInfinity;
            closestMinionNotInGroup = null; closestMinionNotInGroupDist = float.PositiveInfinity;
            closestUsableBuilding = null; closestUsableBuildingDist = float.PositiveInfinity;
            autoAimTarget = null; autoAimTargetAngle = float.PositiveInfinity;



        }
        /*public override Matrix[] GetSkinTransforms()
        {
            Matrix[] baseTrans = base.GetSkinTransforms();
            //Console.WriteLine(bobAmount);


            for(int i = 0; i < baseTrans.Length; i++){
                baseTrans[i] *= Matrix.CreateTranslation(new Vector3(0, bobAmount, 0));
            }
            
            return baseTrans;
        }*/
        public override void Damage(float amount)
        {
            
            base.Damage(amount);
            RumbleManager.StartVibration(Owner.GetPlayerIndex(), amount / maxHitPoints, amount / maxHitPoints, 250);
        }

        public static void SetSurroundings(Surroundings aio)
        {
            playerSurroundings = aio;
        }

        public void UpdateDestination(GameTime gameTime)
        {
            //Camera camera = DrawManager.GetInstance().leftCamera;

            /*if (camera.ChaseMode() && camera.Following == this)
            {
                angleAround.Y += MathHelper.ToRadians(-input.DpadVector.X * 2);
                MoveDestination((GetPosition() - Calculations.V3toV2(camera.GetPosition())) / 20 * -input.DpadVector.Y);
            }
            else
            {*/

            if (velocity != Vector2.Zero)
           {
                float target = (float)(Math.Atan2(-velocity.Y, velocity.X) + Math.PI / 2);
                while (Math.Abs(rotation - target) > Math.PI + 0.1)
                {
                    if (target > rotation)
                    {
                        target -= 2 * (float)Math.PI;
                    }
                    else
                    {
                        target += 2 * (float)Math.PI;
                    }

                }
                rotation  = Calculations.Interpolate(rotation, target, 0.12f);
                

            }



            angleAround.Y = rotation;

            if (velocity.X != 0)
            {
               // tiltAngle.X = Calculations.Interpolate(tiltAngle.X, -velocity.X / Math.Abs(velocity.X)/6, 0.1f);
                tiltAngle.X = Calculations.Interpolate(tiltAngle.X, -velocity.X / 24, 0.1f);
            }
            else
            {
                tiltAngle.X = Calculations.Interpolate(tiltAngle.X, 0, 0.1f);
            }
            if (velocity.Y != 0)
            {
                tiltAngle.Y = Calculations.Interpolate(tiltAngle.Y, velocity.Y / 24, 0.1f);
            }
            else
            {
                tiltAngle.Y = Calculations.Interpolate(tiltAngle.Y, 0, 0.1f);
            }
            angleAround.Z = tiltAngle.X;
            angleAround.X = tiltAngle.Y;
            
            
           // angleAround.Z = -velocity.X;

            MoveDestination(velocity * gameTime.ElapsedGameTime.Milliseconds / 1000);
        }

        public override void HandlePossibleInteraction(IInteractable other)
        {
            /*if (other is MetalResource &&
                    Calculations.IsWithin2DRange(GetPosition(), other.GetPosition(), 1.0f))
            {
                
                SoundPlayer.PlaySound("coin");
                ((MetalResource)other).RemoveFromGame();
                Owner.AddMetal(((MetalResource)other).GetMetalValue());
            }*/




            
            if (!Owner.BuildMenuOpen() && other is IUsableStructure)
            {
                if (other is Building)
                {
                    if (((Building)other).Owner == Owner && !((Building)other).IsUnderConstruction())
                    {
                        float distSquared = Vector2.DistanceSquared(this.GetPosition(), other.GetPosition());
                        if (distSquared < MAX_CONTROL_DISTANCE && distSquared < closestUsableBuildingDist)
                        {
                            closestUsableBuildingDist = distSquared;
                            closestUsableBuilding = (IUsableStructure)other;
                        }
                    }
                }
                else
                {
                    float distSquared = Vector2.DistanceSquared(this.GetPosition(), other.GetPosition());
                    if (distSquared < MAX_CONTROL_DISTANCE && distSquared < closestUsableBuildingDist)
                    {
                        closestUsableBuildingDist = distSquared;
                        closestUsableBuilding = (IUsableStructure)other;
                    }
                }

            }
            if (Owner.BuildMenuOpen() && other is BuildingNode)
            {
                if (((BuildingNode)other).IsFree())
                {
                    float distSquared = Vector2.DistanceSquared(this.GetPosition(), other.GetPosition());
                    if (distSquared < MAX_CONTROL_DISTANCE && distSquared < closestBuildingNodeDist)
                    {
                        closestBuildingNodeDist = distSquared;
                        closestBuildingNode = (BuildingNode)other;
                    }
                }
            }
            if (other is Minion)
            {
                if (!(((Minion)other).IsInGroup()))
                {
                    float distSquared = Vector2.DistanceSquared(this.GetPosition(), other.GetPosition());
                    if (distSquared < MAX_CONTROL_DISTANCE && distSquared < closestMinionNotInGroupDist)
                    {
                        closestMinionNotInGroupDist = distSquared;
                        closestMinionNotInGroup = (Minion)other;
                    }

                }

            }
            if (other is PlayerOwnedObject)
            {
                PlayerOwnedObject ownable = (PlayerOwnedObject)other;
                if (ownable.Owner != Owner && Calculations.IsWithin2DRange(this.GetPosition(), other.GetPosition(), 10))
                {
                    //Calculate difference in angles between player angle and angle to possible target
                    //TODO Fixa ordentligt sedan!!!
                    float playerAngle = angleAround.Y;
                    Vector2 playerPosition = this.GetPosition();
                    Vector2 otherPosition = other.GetPosition();
                    Vector2 difference = otherPosition - playerPosition;
                    float angleToTarget = (float)Math.PI/2 - (float)Math.Atan2(difference.Y, difference.X);
                    
                    float angleDifference = Math.Abs(playerAngle-angleToTarget);

                    
                    if (angleDifference < autoAimTargetAngle && angleDifference < Calculations.double_Pi4th)
                    {
                        
                        autoAimTargetAngle = angleDifference;
                        autoAimTarget = (GameObject)other;
                    }
                }

            }




           
        }


        float maxY = 2;
        float minY = 2;

        public void UpdateLogic(GameTime gameTime)
        {
            HandleInput();
            //Guess buildings add energy, but some should be added by hero otherwise he will run out!
            Owner.AddEnergy(1);
            UpdateGroup();//TODO Maybe find better placement?
            bobAmount = (float)Math.Sin((float)gameTime.TotalGameTime.Milliseconds / 1000);

            Matrix[] skinTransforms = ModelComp.GetSkinTransforms();
            //Console.WriteLine(skinTransforms[0]);
            Vector3 actualPos = GetPositionV3();
            //Vector3 withAnimPos =
           // Vector3 translate = secondaryAnimationPlayer.GetBoneTransforms()[0].Translation;
           /* if (translate.Y > maxY)
            {
                maxY = translate.Y;
            }
            if (translate.Y < minY)
            {
                minY = translate.Y;
            }*/

            //Console.WriteLine(translate);
            // 

            ParticleManager.GetInstance().AddLight(
                this.GetPositionV3() + new Vector3(0, 1, 0), new Vector4(0.5f, 0.5f, 0.2f, 0),
                1.0f * DayCycle.GetNightLight(),
                6,
                0.001f,
                true);
        }

        public override CollidableType GetCollidableType()
        {
            return CollidableType.Hero;
        }


        public override float GetSolidCollisionSize()
        {
            
            return base.GetSolidCollisionSize() / 93;
        }

        public override MovementStrategy GetObstacleHandlingStrategy()
        {
            return MovementStrategy.SearchPathForPlayer;
        }

        public override float GetMaxInteractionRange()
        {
            return 20;
        }


        public override void HitPointsZero()
        {
            
            // Since we are not interested in moving from position towards destination,
            // set both to the target destination so
            // that the player winds up there no matter what

            if (Owner.MainBase != null)
            {
                Teleport(Owner.FindClosestRespawn(this.GetPosition()));
                ParticleManager.GetInstance().AddExplosion(GetPositionV3(), Calculations.V2ToV3(velocity, height), 1);
                SoundPlayer.Play3DSound("explosion", this.GetPositionV3());
                hitPoints = 1;
                Owner.SubtractEnergy(Owner.GetEnergy());
                minionGroup.Disband();
                animationPlayer.StartClip("SpecialMove2", false, 1.0f);
            }
            else
            {
                throw new Exception("Game over");
            }

            /*
            if (Owner.GetPlayerIndex() == PlayerIndex.One)
            {
                Teleport(Owner.FindClosestRespawn(this.GetPosition(), Constants.Player1StartingPosition));
            }
            else
            {
                Teleport(Owner.FindClosestRespawn(this.GetPosition(), Constants.Player1StartingPosition));
            }*/
        }
        public Group GetGroup()
        {
            return minionGroup;
        }
        public void UpdateGroup()
        {
            minionGroup.UpdatePos(this.GetPosition());
            minionGroup.UpdaterXrY(this.GetPosition());
        }





        public void updateAnimation(GameTime gameTime)
        {
            animationPlayer.Update(gameTime.ElapsedGameTime);
            secondPlayer.Update(gameTime.ElapsedGameTime);
        }
    }
}
