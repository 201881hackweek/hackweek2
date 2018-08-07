using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading; //导入命名空间,类Thread就在此空间中
 



public class SceneEventManager : MonoBehaviour
{
    public GameObject Blackscreen;
    public Text Blackscreentext;
    public GameObject Dialogbox;
    public Text Dialogboxtext;
    public int blacknumber = 0;//不同的黑屏触发点有不同的编号
    public int generalnumber = 0;//不同的普通触发点有不同的编号
    private int textnumber = 0;
    public bool space = false;
    public float blackfinishtime;
	public float generalfinishtime;
	
	// 1:开场1
	private string[] black1text =
	{
		"黑暗。我从一片黑暗里醒来h",
		"黑暗。我从一片黑暗里醒来h不行。不能再这样等着了h",
		"黑暗。我从一片黑暗里醒来h不行。不能再这样等着了h某个人对我说道h我要逃。越快越好h",
	};
	
	// 2:开场2
    private string[] black2text = 
	{ 
		"我是孤独的h", 
		"我是孤独的h人们所说的那些属于普通人的幸福h",
		"我是孤独的h人们所说的那些属于普通人的幸福h家人，朋友，恋人h",
		"我是孤独的h人们所说的那些属于普通人的幸福h家人，朋友，恋人h对我来说都过于遥远，和书上密密麻麻排列的符号一样抽象h",
		"但是，那又如何？h",
		"但是，那又如何？h狼不需要像兔子一样吃草，我生来就不需要这种柔软黏腻的东西h",
		"但是，那又如何？h狼不需要像兔子一样吃草，我生来就不需要这种柔软黏腻的东西h我所在意的只有一个h",
		"让我活下去h",
	};

	// 3:衣柜
    private string[] dialog3text =
    {
        "有时候，当没有人注意我的时候h",
        "我会缩到柜子里头h",
        "我会缩到柜子里头h那里很好h",
        "狭窄、阴暗、安静h",
        "最重要的是，当缩进旧衣服堆时h",
        "我会有一种温暖的错觉h",
        "就好像...有人...在拥抱着我......h"
    };
	
	// 4:镜子
    private string[] dialog4text =
	{
		"镜子：“……他现在还没有丝毫记忆……”h",	
	};
	
	// 6:电视机
	private string[] dialog6text = 
	{
		"屏幕一闪一闪的，没信号了吧h",
	};
	
	// 8:发现BOSS1
    private string[] dialog8text =
    {
		"！！母亲？！h",
		"但是好可怕……h",
		"我要躲开它才行……h",
    };
	
	// 13:餐桌
	private string[] dialog13text = 
	{
		"布满了血迹，还有一把菜刀h",
	};
	
	// 14:浴缸文案
	private string[] dialog14text = 
	{
		"里面有一缕缕黏腻的头发，好恶心h",
	};
	
	// 15:浴缸放水
    private string[] dialog15text =
    {
		"排水口这儿堵着，我分不出来这是什么。放个水吧h",
		"呀，是头发！头发里面好像有什么东西h",
	};
	
	// 19_1:爸爸的书房，用钥匙开柜子
	private string[] dialog19text = 
	{
		"父亲的书房：锁了……h",
	};
	// 19_2:爸爸的书房，用钥匙打开了柜子
	private string[] dialog20text = 
	{
		"父亲的工作资料全在这儿,在书柜里找到一把钥匙h",
	};
	
	// 21:父亲的书柜
	private string[] dialog21text = 
	{
		"其实只要仔细翻就会发现……h",
		"所有的书靠里那边都浸满了血h",
	};
	
	// 22:母亲的房间
	private string[] dialog22text = 
	{
		"母亲的房间：锁了……",
	};
	
	// 23:母亲剧情（PS：有图片）
	private string[] black23text =
    {
		"那双眼睛盯着我h",
		"那双眼睛盯着我h这时，我正因为疼痛而尖叫、打滚、嚎哭h",
		"但当我看到那双眼睛时，一切都无所谓了h",
		"但当我看到那双眼睛时，一切都无所谓了h它们是那么冰冷,看着我时好像在看路边的一块石头h是既没有爱怜、也没有恶意的——绝对的漠视h",
		"但当我看到那双眼睛时，一切都无所谓了h它们是那么冰冷,看着我时好像在看路边的一块石头h是既没有爱怜、也没有恶意的——绝对的漠视h从脊背上行的寒意将我整个人都冰冻了起来h",
		"为什么呢？你不应该帮助我吗？你不应该保护我吗？h",
		"为什么呢？你不应该帮助我吗？你不应该保护我吗？h你不应该……爱我吗？h",
		"为什么呢？你不应该帮助我吗？你不应该保护我吗？h你不应该……爱我吗？h如果这些都没有的话……h",
		"为什么呢？你不应该帮助我吗？你不应该保护我吗？h你不应该……爱我吗？h如果这些都没有的话……h为什么你要生下我呢？h",
	};

	// 24:眼睛文案（PS：有图片）
	private string[] black24text = 
	{
		"“你应该报警”我这样对那双眼睛的主人说道h",
		"“你应该报警”我这样对那双眼睛的主人说道h“为了我们两个人好。”h",
		"她麻木地用碘酒擦拭着嘴角的伤处，“没用的。”h",
		"她麻木地用碘酒擦拭着嘴角的伤处，“没用的。”h嘴唇一张一合，连带着旁边胆黄色的药迹也变成了滑稽的弧度，“他不会被逮捕的。”h",
		"“而且，我一个人活不下去。”h",
		"“而且，我一个人活不下去。”h她转过身来，盯着我。是那种我最为痛恨的漠然眼神h",
		"“而且，我一个人活不下去。”h她转过身来，盯着我。是那种我最为痛恨的漠然眼神h“所以，还请你好好忍耐下去。”她说道，h",
		"“而且，我一个人活不下去。”h她转过身来，盯着我。是那种我最为痛恨的漠然眼神h“所以，还请你好好忍耐下去。”她说道，h“……为了我们两个人好。”h",
		"我拧出一个微笑。把咯咯磨牙声吞进肚子里。h",
		"我拧出一个微笑。把咯咯磨牙声吞进肚子里。h啊啊，即使是现在回忆起来，愤怒也丝毫没有消退。h",
		"我拧出一个微笑。把咯咯磨牙声吞进肚子里。h啊啊，即使是现在回忆起来，愤怒也丝毫没有消退。h但我决定原谅她。h",
		"我拧出一个微笑。把咯咯磨牙声吞进肚子里。h啊啊，即使是现在回忆起来，愤怒也丝毫没有消退。h但我决定原谅她。h谁知道呢？毕竟你没必要与一堆腐肉较劲。h",
	};
	
	// 27:杀死BOSS1后的CG（黑屏部分）
	private string[] black27text =
	{
		"眼睛消失了。大概是逃跑到什么地方去了吧h",
		"眼睛消失了。大概是逃跑到什么地方去了吧h本就缺乏温度的地狱里变得更加冷清了h",
		"眼睛消失了。大概是逃跑到什么地方去了吧h本就缺乏温度的地狱里变得更加冷清了h此时我才发现，我竟然怀念那双冰冷的眼睛h",
		"即使她再如何漠不关心h",
		"即使她再如何漠不关心h至少在某些时候，我们是盟友h",
	};
	// 27:CG（对话部分）
	private string[] black28text =
	{
		"'母亲'消失了，'父亲'……来了……h",
		"普通子弹伤害不了他……要去找他的弱点……h",
		"他的弱点，在他房间里肯定有线索！h",
	};

	// 28:BOSS死亡后得到日记

	// 30_1:窗户
	private string[] dialog30text =
	{
		"窗户：站在这里，可以看见街道h",
	};
	
	// 30_2:街上（PS：有图片）
	private string[] dialog29text =
	{
		"我跑到街上h",
		"但是，其他人盯着我的时候，依旧是那样冷漠的眼神h",
	};
	
	// 31:自己的日记
	private string[] dialog31text = 
	{
		"自己的日记：我平时有写日记的习惯，虽然我的生活没什么好写的。h",
		"父亲和母亲都写日记。写什么呢？记录这一家扭曲的生活吗？h",
	};
	
	// 32:查看
	private string[] dialog32text = 
	{
		"疼痛。这对我而言不算陌生h",
		"伤害撞击在肉体上，就会产生这种强烈的电流h",
		"它们确实令人不快，但我更愿意用“麻烦”来形容h",
		"不行。我的身体太虚弱了，没法承受太多的伤痕h",
		"这样下去，我一定会变成一团没有意识的血肉，失去一切活力和生机h",
		"这里是我的地狱。是连逃跑的意志都会消磨殆尽的万丈深渊h",
		"所以——必须、要摆脱这一切不可。”h",
	};
    
	// 33:文案
	private string[] dialog33text = 
	{
		"“今天父亲将母亲杀死了。h",
		"“头颅藏在一个箱子里。然后父亲将箱子放在了那个房间……h",
		"“那个房间是连结我和学校的唯一通道h",
		"尽管我不能上学，单是每天在窗边看着快乐的学生们，就能唤起我仅存的一点希望。”h",
	};

	// 34_1:房间落灰的箱子
	private string[] dialog58text = 
	{
		"落灰的箱子：看样子它被放置在这里很久了h",
	};
	// 34_2:房间落灰的箱子（打开）
	private string[] dialog59text = 
	{
		"打开：噫——是母亲的头h",
	};
	// 34_3:房间落灰的箱子（抠挖）
	private string[] dialog60text =
	{
		"箱子：果然啊……钥匙藏在这里，黑暗，粘稠，充满血液h",
	};
	
	// 35:父亲杀母亲的剧情（PS：有很多图片）
	private string[] black35text =
	{
		"恶魔的手高高举起，又重重落下h",
		"恶魔的手高高举起，又重重落下h女人腥臭的体液随着运动的幅度溅起h",
		"恶魔的手高高举起，又重重落下h女人腥臭的体液随着运动的幅度溅起h四处都是刺眼的红色。这片地狱从未这么像地狱h",
		"那双冰冷的眼睛现在失去生气了，从眼眶里掉下来h",
		"那双冰冷的眼睛现在失去生气了，从眼眶里掉下来h连着红蓝色的神经线h",
		"那双冰冷的眼睛现在失去生气了，从眼眶里掉下来h连着红蓝色的神经线h机械的滚到我的脚边h",
		"我再也无法忍耐了h",
		"我再也无法忍耐了h我爆发出比任何时候都要大声的尖叫，跑上了街道h",
	};
	
	// 37:右边窗户
	private string[] black37text = 
	{
		"学校,他们聚集起来h",
		"学校,他们聚集起来h那些与我身高相仿，带着满足微笑的幼崽，蹦跳着地聚作一团，像一群叽叽喳喳的小麻雀h",
		"我从窗外往外望去，看到的就是这样的景象h",
		"我从窗外往外望去，看到的就是这样的景象h那是我愿意付出一切换取的温暖h",
	};
	
	// 38:右边窗户与父亲的对白
	private string[] black38text = 
	{
		"“为什么我不可以去上学？”我问恶魔h",
		"“为什么我不可以去上学？”我问恶魔h“他似乎愣了一下。随后，嗤笑一声：“你别打鬼主意。”h",
		"“为什么我不可以去上学？”我问恶魔h“他似乎愣了一下。随后，嗤笑一声：“你别打鬼主意。”h“我没有。”我平静地回答道h",
		"“为什么我不可以去上学？”我问恶魔h“他似乎愣了一下。随后，嗤笑一声：“你别打鬼主意。”h“我没有。”我平静地回答道h“我只是想上学。”h",
		"“你……想？”恶魔夸张地挑起一边眉毛。h",
		"“你……想？”恶魔夸张地挑起一边眉毛。h是的。我……非常想。h",
		"“你……想？”恶魔夸张地挑起一边眉毛。h是的。我……非常想。h恶魔哈哈大笑起来。而后，揪住我的头发，狠狠砸在墙上。h",
		"“你……想？”恶魔夸张地挑起一边眉毛。h是的。我……非常想。h恶魔哈哈大笑起来。而后，揪住我的头发，狠狠砸在墙上。h“你别想逃跑！”他嘶吼道h",
		"“你……想？”恶魔夸张地挑起一边眉毛。h是的。我……非常想。h恶魔哈哈大笑起来。而后，揪住我的头发，狠狠砸在墙上。h“你别想逃跑！”他嘶吼道h“想都别想！”h",
		"又来了。我闭上眼睛h",
	};
	
	// 39:床（PS：有图片）
	private string[] black39text =
	{
		"眼睛消失了。这本该是件好事h",
		"眼睛消失了。这本该是件好事h但我无论如何也没法摆脱焦躁。h",
		"眼睛消失了。这本该是件好事h但我无论如何也没法摆脱焦躁。h因为那双饱含着恶意的眼睛。如有实质地黏在我的后背h",
		"恶魔发红的眼睛直直瞪向这边，但与平常的疯狂又有所不同。h",
		"恶魔发红的眼睛直直瞪向这边，但与平常的疯狂又有所不同。h他究竟在想什么？h",
		"他究竟要干什么？h"
	};
	
	// 40:床（CG？）
	private string[] black40text = 
	{
		"我能回忆起最久远、也是最鲜明的记忆，是疼痛h",
		"我能回忆起最久远、也是最鲜明的记忆，是疼痛h手上；腿上；肩上；背上。钝器；拳头；烟头；尖刀。每一寸裸露的皮肤都布满伤口，血腥在空气里蔓延，我的房间里全是暴力和侵犯留下的苦味。手指能够触碰到的只有疼痛、疼痛、疼痛h",
		"这里是我的地狱。是连逃跑的意志都会消磨殆尽的万丈深渊h",
		"这里是我的地狱。是连逃跑的意志都会消磨殆尽的万丈深渊h恶魔栖居我的身侧h",
		"这里是我的地狱。是连逃跑的意志都会消磨殆尽的万丈深渊h恶魔栖居我的身侧h他是气味的源头，是地狱的中心，是一切恐惧和不幸的来源h",
		"好痛！好痛！好痛！h",
		"好痛！好痛！好痛！h他怎么敢？！他怎么敢？！h",
		"啊啊。他要过来了h",
		"啊啊。他要过来了h又一次将疼痛无端施加在我身上h",
		"啊啊。他要过来了h又一次将疼痛无端施加在我身上h在红色散落出来之前，我闭上眼睛，默默祈祷着h请让他消失吧h",
	};
	
	// 41:父亲杀母亲
	private string[] black41text = 
	{
		"父亲杀母亲文案“你什么都看见了？”恶魔嘶哑的声音问道h",
		"父亲杀母亲文案“你什么都看见了？”恶魔嘶哑的声音问道h我睁开眼，为眼前的一片狼藉而皱起眉头h",
		"父亲杀母亲文案“你什么都看见了？”恶魔嘶哑的声音问道h我睁开眼，为眼前的一片狼藉而皱起眉头h这样不优雅的造物。真够恶心。我要吐了h",
		"但我马上意识到，这同时是一个机会h“需要我帮忙吗？”我冷静地问道h",
		"但我马上意识到，这同时是一个机会h“需要我帮忙吗？”我冷静地问道h恶魔愣了一下，继而，宛若乌鸦般粗哑地嘎嘎笑了起来h",
		"但我马上意识到，这同时是一个机会h“需要我帮忙吗？”我冷静地问道h恶魔愣了一下，继而，宛若乌鸦般粗哑地嘎嘎笑了起来h“好——你果然是我的儿子！”h",
		"但我马上意识到，这同时是一个机会h“需要我帮忙吗？”我冷静地问道h恶魔愣了一下，继而，宛若乌鸦般粗哑地嘎嘎笑了起来h“好——你果然是我的儿子！”h——和你没有关系，我只是想活下去而已h",
		"我将紧紧绑着的黑色塑料袋丢进火堆里，摘下棉布手套，一起扔了进去h闻着空气中略微的焦味，我忍不住笑了一下h麻烦清除一个了h",
		"我将紧紧绑着的黑色塑料袋丢进火堆里，摘下棉布手套，一起扔了进去h"
	};
	
	// 43:击败父亲
	private string[] black43text = 
	{
		"我闭上眼睛，又睁开。眼前是全然陌生的景象h",
		"我闭上眼睛，又睁开。眼前是全然陌生的景象h我不知道我在哪里，但这没关系h",
		"我闭上眼睛，又睁开。眼前是全然陌生的景象h我不知道我在哪里，但这没关系h我知道：我必须要逃h",
		"唯一令我动摇的，是记忆中他惊恐的双眼h",
		"唯一令我动摇的，是记忆中他惊恐的双眼h是了，他现在一定把一切都忘记了吧h",		"唯一令我动摇的，是记忆中他惊恐的双眼h是了，他现在一定把一切都忘记了吧h但是没关系，即使是忘记了，只要我能够活下去就行了h",
		"唯一令我动摇的，是记忆中他惊恐的双眼h是了，他现在一定把一切都忘记了吧h但是没关系，即使是忘记了，只要我能够活下去就行了h为此我不惜一切代价h"
	};
	
	// 44:击败父亲后的CG
	private string[] black44text = 
	{
		"我看见尸体。被分成无数细碎小片的尸体h",
		"我看见尸体。被分成无数细碎小片的尸体h到处都是令人作呕的红色。我摇晃着后退两步，滑落在地上h",
		"我看见尸体。被分成无数细碎小片的尸体h到处都是令人作呕的红色。我摇晃着后退两步，滑落在地上h某个金属物体从我的指尖掉下，划过地面发出沉重的声音h",
		"我看见尸体。被分成无数细碎小片的尸体h到处都是令人作呕的红色。我摇晃着后退两步，滑落在地上h某个金属物体从我的指尖掉下，划过地面发出沉重的声音h我低头，那是一把沾满血迹的削骨刀h",
		"——等一下h",
		"——等一下h我颤抖着抬起手来，看见指尖不断滑落的、不属于我的温热鲜血h",
		"——等一下h我颤抖着抬起手来，看见指尖不断滑落的、不属于我的温热鲜血h为什么我会在这里h",
		"——等一下h我颤抖着抬起手来，看见指尖不断滑落的、不属于我的温热鲜血h为什么我会在这里h为什么——我会拿着凶器h",
	};
	
	// 45:父亲的日记
	private string[] dialog45text =
	{
		"父亲消失了。刚刚慌乱中瞄到父亲卧室桌上有本日记h",
		"父亲的日记：我看到儿子做的一切了h",
		"他以为将动物尸体藏在顶楼就没人发现了?",
		"上次去翻还发现了很多火柴盒……h",
		"啊……有我这样一个父亲h",
		"他这一辈子，都无法见到光明了h",
	};
	
	// 51:柜子里的动物尸体
	private string[] dialog51text = 
	{
		"里面装满了动物尸体……原来这就是我的宝藏么？可是我完全没有记忆……",
	};
	
	// 52:小动物情节
	private string[] black52text = 
	{
		"焦躁席卷我的全身h就像把我丢在七月的柏油马路上，炙烤着每一寸皮肤h",
		"焦躁席卷我的全身h就像把我丢在七月的柏油马路上，炙烤着每一寸皮肤h每当我再也忍不住的时候，我就会把刀、汽油和火柴装进包里，冲去一个没有人的地方h",
		"我抓住生物。他们略高的体温让我满足h我把它们的挣扎掐死在一道毫不犹豫的寒光下h然后剖开肚子，看五颜六色的内脏像丝绸一样滑过我的手指掉在地上h",
		"我抓住生物。他们略高的体温让我满足h我把它们的挣扎掐死在一道毫不犹豫的寒光下h然后剖开肚子，看五颜六色的内脏像丝绸一样滑过我的手指掉在地上h直至这一团温暖的血肉变得冰冷，我淋上汽油、点燃，欣赏炙热的火舌与冉冉升起的黑烟h",
		"无关仇恨，无关悲伤h",
		"无关仇恨，无关悲伤h只有这样我才能获得短暂的平静h",
	};
	
	// 55:房间获得父亲的碎片
	private string[] dialog55text = 
	{
		"不行，我实在找不到钥匙了。但是这个房间的锁很旧了，应该可以找个什么东西撬开h",
	};
	// 55:房间获得父亲的碎片（用剔骨刀）
	private string[] dialog54text =
	{
		"打开了。父亲的碎片：为什么尸块在这里？必须赶快销毁……有什么好的销毁方式吗，不留痕迹的那种？",
	};
	
	// 56:烧碎片 & 58:与警察战斗
	private string[] dialog56text = 
	{
		"好了，证据被销毁了，我可以直面警察了！h",
	};
	
	// 57:父母的头发纠缠在一起
	private string[] dialog57text = 
	{
		"爸爸和妈妈，纠缠在一起……h"
	};
	
	// 59:干掉警察后的CG
	private string[] black59text = 
	{
		"我闭上眼睛，又睁开。眼前是全然陌生的景象h",
		"我闭上眼睛，又睁开。眼前是全然陌生的景象h我在哪里？我为什么会在这个地方？h",
		"我闭上眼睛，又睁开。眼前是全然陌生的景象h我在哪里？我为什么会在这个地方？h记忆混乱而模糊。理由已经忘记，我只记得：我必须要逃h",
		"但究竟是谁告诉了我这一切？h",
		"但究竟是谁告诉了我这一切？h那片混乱的红色……又究竟是谁做的？h",
	};
	
	// 60:里人格的独白
	private string[] black60text = 
	{
		"我不喜欢亲自动手杀人h",
		"我不喜欢亲自动手杀人h大部分时候，它是风险最高、最为低效的解决方案h",
		"我不喜欢亲自动手杀人h大部分时候，它是风险最高、最为低效的解决方案h如果有其他的选择，我绝不会选择这一条道路h",
		"但是他不一样h",
		"但是他不一样h他需要鲜血。他需要杀戮h",
		"但是他不一样h他需要鲜血。他需要杀戮h他需要看着鲜活的生命在手中流逝h",
		"所以他才走上了这条最为狭窄的道路h",
		"所以他才走上了这条最为狭窄的道路h但是，没关系h",
		"所以他才走上了这条最为狭窄的道路h但是，没关系h因为我的使命就是保护他h",
		"所以他才走上了这条最为狭窄的道路h但是，没关系h因为我的使命就是保护他h那是比一切都要深刻的羁绊，那是我存在于世间唯一的意义h",
		"所以他才走上了这条最为狭窄的道路h但是，没关系h因为我的使命就是保护他h那是比一切都要深刻的羁绊，那是我存在于世间唯一的意义h——我想要他自由h",
	};
	
	void Update()
    {
        Blackchoose();
        Generalchoose();
    }

	
    public void Blackchoose()
    {
        if (blacknumber == 0) return;
        switch (blacknumber)
        {
            case 1:
                blacknext(black1text);
                break;
            case 2:
                blacknext(black2text);
                break;
			case 23:
				blacknext(black23text);
                break;
			case 24:
				blacknext(black24text);
                break;
			case 27:
				blacknext(black27text);
                break;
			case 28:
				blacknext(black28text);
                break;
			case 35:
				blacknext(black35text);
                break;
			case 37:
				blacknext(black37text);
				blacknext(black38text);
                break;
			case 39:
				blacknext(black39text);
                break;
			case 40:
				blacknext(black40text);
                break;
			case 41:
				blacknext(black41text);
                break;
			case 43:
				blacknext(black43text);
                break;
			case 44:
				blacknext(black44text);
                break;
			case 52:
				blacknext(black52text);
                break;
			case 59:
				blacknext(black59text);
                break;
			case 60:
				blacknext(black60text);
                break;
        }
    }

    public void Generalchoose()
    {
        if (generalnumber == 0) return;
        switch (generalnumber)
        {
            case 3:
                generalnext(dialog3text);
                break;
            case 4:
                generalnext(dialog4text);
                break;
			case 6:
				generalnext(dialog6text);
				break;
			case 8:
                generalnext(dialog8text);
                break;
			case 13:
                generalnext(dialog13text);
                break;
			case 14:
                generalnext(dialog14text);
                generalnext(dialog15text);
                break;
			case 19:
                generalnext(dialog19text);
                break;
			case 20:
                generalnext(dialog20text);
                break;
			case 21:
                generalnext(dialog21text);
                break;
			case 22:
                generalnext(dialog22text);
                break;
			case 30:
                generalnext(dialog30text);
                break;
	        case 29:
                generalnext(dialog29text);
                break;
			case 32:
                generalnext(dialog32text);
                break;
			case 33:
                generalnext(dialog33text);
                break;
			case 58:
                generalnext(dialog58text);
                generalnext(dialog59text);
                generalnext(dialog60text);
                break;
			case 45:
                generalnext(dialog45text);
                break;
			case 51:
                generalnext(dialog51text);
                break;
			case 55:
                generalnext(dialog55text);
                break;
			case 54:
                generalnext(dialog54text);
                break;
			case 56:
                generalnext(dialog56text);
                break;
			case 57:
                generalnext(dialog57text);
                break;
			}
    }

    void general1next()
    {
        if (space)
        {
            Blackscreentext.text = black1text[0];//如果是用空格键触发的 那么先单独显示第一条（如果是鼠标触发的不存在此问题，因为该帧调用此函数时Getmouseup检测通过）
            Blackscreentext.text = Blackscreentext.text.Replace("h", "\n");
            textnumber = 1;//textnumber手动加1
            space = false;//下次调用不再进入
        }
        if (textnumber < 7)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Blackscreentext.text = black1text[textnumber];
                Blackscreentext.text = Blackscreentext.text.Replace("h", "\n");//text的换行方法
                textnumber++;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Blackscreen.SetActive(false);
                textnumber = 0;
                blacknumber = 0;
            }
            blackfinishtime = Time.time;
        }
    }
    void general2next() { }

    void black1next()
    {
        if (space)
        {
            Blackscreentext.text = black1text[0];//如果是用空格键触发的 那么先单独显示第一条（如果是鼠标触发的不存在此问题，因为该帧调用此函数时Getmouseup检测通过）
            Blackscreentext.text = Blackscreentext.text.Replace("h", "\n");
            textnumber = 1;//textnumber手动加1
            space = false;//下次调用不再进入
        }
        if (textnumber < 7)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Blackscreentext.text = black1text[textnumber];
                Blackscreentext.text = Blackscreentext.text.Replace("h", "\n");//text的换行方法
                textnumber++;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Blackscreen.SetActive(false);
                textnumber = 0;
                blacknumber = 0;
            }
            blackfinishtime = Time.time;
        }
    }


    void black2next()
    {
        if (textnumber < 4)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Blackscreentext.text = black2text[textnumber];
                Blackscreentext.text = Blackscreentext.text.Replace("h", "\n");
                textnumber++;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Blackscreen.SetActive(false);
                textnumber = 0;
                blacknumber = 0;
            }

        }
    }
    
    void blacknext(string [] ARRAY)
    {
		int LENGTH = ARRAY.Length;
        if (space)
        {
            Blackscreentext.text = ARRAY[0];//如果是用空格键触发的 那么先单独显示第一条（如果是鼠标触发的不存在此问题，因为该帧调用此函数时Getmouseup检测通过）
            Blackscreentext.text = Blackscreentext.text.Replace("h", "\n");
            textnumber = 1;//textnumber手动加1
            space = false;//下次调用不再进入
        }
        if (textnumber < LENGTH)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Blackscreentext.text = ARRAY[textnumber];
                Blackscreentext.text = Blackscreentext.text.Replace("h", "\n");//text的换行方法
                textnumber++;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Blackscreen.SetActive(false);
                textnumber = 0;
                blacknumber = 0;
            }
            blackfinishtime = Time.time;
        }
    }
	
	void generalnext(string [] ARRAY)
    {
		int LENGTH = ARRAY.Length;
        if (space)
        {
            Dialogboxtext.text = ARRAY[0];//如果是用空格键触发的 那么先单独显示第一条（如果是鼠标触发的不存在此问题，因为该帧调用此函数时Getmouseup检测通过）
            Dialogboxtext.text = Dialogboxtext.text.Replace("h", "\n");
            textnumber = 1;//textnumber手动加1
            space = false;//下次调用不再进入
        }
        if (textnumber < LENGTH)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogboxtext.text = ARRAY[textnumber];
                Dialogboxtext.text = Dialogboxtext.text.Replace("h", "\n");//text的换行方法
                textnumber++;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogbox.SetActive(false);
                textnumber = 0;
                generalnumber = 0;
            }
		generalfinishtime = Time.time;
        }
    }
}
