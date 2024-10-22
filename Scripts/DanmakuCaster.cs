    // Start of Selection
    using Godot;
    using System;
    using System.Threading.Tasks;

    public partial class DanmakuCaster
    {


        public static void setBullet(Vector2 velocity, Vector2 position, Entity caster){
            PackedScene bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
            Bullet bullet = bulletScene.Instantiate<Bullet>();
            bullet.velocity = velocity;
            bullet.GlobalPosition = position;
            bullet.caster = caster;
            GameScene.instance.AddChild(bullet);
        }

        public static void playIndicatorAt(Vector2 position, Action callback){
            PackedScene danmakuIndicatorScene = GD.Load<PackedScene>("res://Scenes/DanmakuIndicator.tscn");
            DanmakuIndicator danmakuIndicator = danmakuIndicatorScene.Instantiate<DanmakuIndicator>();
            danmakuIndicator.GlobalPosition = position;
            danmakuIndicator.AddAnimationFinishedCallback(callback);
            GameScene.instance.AddChild(danmakuIndicator);
        }
        public static async void CastDanmaku(){
            // 定义螺旋弹幕参数
            Vector2 center = GameScene.player.GlobalPosition; // 以玩家位置为中心
            int patternCount = 8; // 圆心点数量
            int bulletCountPerPattern = 24; // 每个圆形弹幕的子弹数量
            float spiralSpacing = 20f; // 螺旋间距
            float initialRadius = 10f; // 螺旋起始半径
            float bulletRadius = 0.1f; // 每个圆形弹幕的半径
            float speed = 75; // 子弹速度
            float angleIncrement = Mathf.Pi / 6; // 螺旋角度增量
            float interval = 0.07f; // 每个圆形弹幕释放之间的时间间隔（秒）
            float initialPhase = 10f; // 螺旋起始相位

            

            for(int i = 0; i < patternCount; i++)
            {
                // 计算螺旋中心点
                float angle = initialPhase + i * angleIncrement;
                float radius = initialRadius + i * spiralSpacing;
                Vector2 spiralCenter = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

                playIndicatorAt(spiralCenter, () => {
                    // 释放一个圆形弹幕
                    for(int j = 0; j < bulletCountPerPattern; j++)
                    {
                    float bulletAngle = j * (2 * Mathf.Pi / bulletCountPerPattern);
                    Vector2 position = spiralCenter + new Vector2(Mathf.Cos(bulletAngle), Mathf.Sin(bulletAngle)) * bulletRadius;
                    Vector2 velocity = (position - spiralCenter).Normalized() * speed;

                        setBullet(velocity, position, GameScene.player);
                    }
                });

                // 等待一段时间再释放下一个圆形弹幕
                await Task.Delay(TimeSpan.FromSeconds(interval));
            }
        }
    }
