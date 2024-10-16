# TODO

## 截止下周三，midterm video前的todo

### 自动化地图生成
地图决定不搞地牢类的，每次开一个新关卡然后直接传送到下一个关卡
看看能不能搞个接口让GPT生成地图

### 修改LivingEntity, 添加MapEntity

LivingEntity 和MapEntity类，继承接口IDamagable, 有一个函数Damage(damageAmount, damagesource = null, damageDirection = (0,0))，getHealth，setHealth，getMaxHealth

MapEntity拥有坚韧，类似博德之门，只有伤害>某个数值时会受到伤害

- 花花草草这个伤害可以很低，一些看起来就很硬的这个数值高一些

所有树、墙壁、花花草草什么的都继承MapEntity

然后可以搞一个配套的猴版玩家物品栏

### 添加更多怪物
比如自曝怪，还有精英怪和boss
弄一个爆炸效果，实现方法是一个爆炸的类来创建制定尺寸的碰撞区域，然后在GaneScene这个类里实现CreateExplosion(vector2 position, int level)函数


### 添加更多法术
让法术有一定实战意义，让怪也可以主动施法
