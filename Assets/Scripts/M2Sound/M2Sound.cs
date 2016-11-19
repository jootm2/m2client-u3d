using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class M2Sound
{
	private static Dictionary<int, string> IndexToFn = new Dictionary<int, string> ();
	private static int offLst = 0;

	/// <summary>
	/// 通过索引获取一个声音
	/// </summary>
	/// <param name="index">声音索引，有些常量可以在当前类找到</param>
	/// <param name="isOffLst">是否是附加音频(附加音频是程序默认使用，不再lst文件中)</param>
	public static AudioClip Get (int index, bool isOffLst)
	{
		if (isOffLst)
			index += offLst;
		string filename = null;
		if (IndexToFn.ContainsKey (index))
			filename = IndexToFn [index];
		else {
			if (index > 20000) {
				index -= 20000;
				filename = string.Format ("M{0:0}-{1:0}", index / 10, index % 10);
			} else if (index < 10000) {

				filename = string.Format ("{0:000}-{1:0}", index / 10, index % 10);
			}
		}
		WAV wav = new WAV (Path.Combine (Path.Combine (SDK.RootPath, "Wav"), filename + ".wav"));
		AudioClip audioClip = AudioClip.Create (filename, wav.SampleCount, 1, wav.Frequency, false);
		audioClip.SetData (wav.LeftChannel, 0);
		return audioClip;
	}

	static M2Sound ()
	{
		string fnLst = Path.Combine (Path.Combine (SDK.RootPath, "Wav"), "sound.lst");
		if (File.Exists (fnLst)) {
			string[] lines = File.ReadAllLines (fnLst);

			for (int i = 0; i < lines.Length; i++) {
				if (string.IsNullOrEmpty (lines [i]) || lines [i] [0] == ';')
					continue;
				
				string[] split = lines [i].Replace (" ", "").Split (':', '\t', ' ', (char)0x9);

				int index;
				if (split.Length <= 1 || !int.TryParse (split [0], out index))
					continue;
				offLst = index;
				if (!IndexToFn.ContainsKey (index))
					IndexToFn.Add (index, split [split.Length - 1].Replace ("wav\\", "").Replace (".wav", ""));
			}
		}
		s_FireFlower_1 = offLst + 1;
		IndexToFn.Add (s_FireFlower_1, "newysound1"); //+1
		s_FireFlower_2 = offLst + 2;
		IndexToFn.Add (s_FireFlower_2, "newysound2"); //+2
		s_FireFlower_3 = offLst + 3;
		IndexToFn.Add (s_FireFlower_3, "newysound-mix"); //+3
		s_HeroLogIn = offLst + 4;
		IndexToFn.Add (s_HeroLogIn, "HeroLogin"); //+4
		s_HeroLogOut = offLst + 5;
		IndexToFn.Add (s_HeroLogOut, "HeroLogout"); //+5
		s_S11 = offLst + 6;
		IndexToFn.Add (s_S11, "S1-1"); // +6
		s_S12 = offLst + 7;
		IndexToFn.Add (s_S12, "S1-2"); // +7
		s_S13 = offLst + 8;
		IndexToFn.Add (s_S13, "S1-3"); // +8
		s_Openbox = offLst + 9;
		IndexToFn.Add (s_Openbox, "Openbox"); //+9
		s_SelectBoxFlash = offLst + 10;
		IndexToFn.Add (s_SelectBoxFlash, "SelectBoxFlash"); //+10
		s_Flashbox = offLst + 11;
		IndexToFn.Add (s_Flashbox, "Flashbox"); //+11
		s_hero_shield = offLst + 12;
		IndexToFn.Add (s_hero_shield, "hero-shield"); //+12
		s_powerup = offLst + 13;
		IndexToFn.Add (s_powerup, "powerup"); //+13

		s_hit_ZRJF_M = offLst + 14;
		IndexToFn.Add (s_hit_ZRJF_M, "M56-0"); //+14
		s_hit_ZRJF_w = offLst + 15;
		IndexToFn.Add (s_hit_ZRJF_w, "M56-3"); //+15


		s_cboZs1_start_m = offLst + 16;
		IndexToFn.Add (s_cboZs1_start_m, "cboZs1_start_m"); //+16
		s_cboZs1_start_w = offLst + 17;
		IndexToFn.Add (s_cboZs1_start_w, "cboZs1_start_w"); //+17
		s_cboZs2_start = offLst + 18;
		IndexToFn.Add (s_cboZs2_start, "cboZs2_start"); //+18
		s_cboZs3_start_m = offLst + 19;
		IndexToFn.Add (s_cboZs3_start_m, "cboZs3_start_m"); //+19
		s_cboZs3_start_w = offLst + 20;
		IndexToFn.Add (s_cboZs3_start_w, "cboZs3_start_w"); //+20
		s_cboZs4_start = offLst + 21;
		IndexToFn.Add (s_cboZs4_start, "cboZs4_start"); //+21

		s_cboFs1_start = offLst + 22;
		IndexToFn.Add (s_cboFs1_start, "cboFs1_start"); //+22
		s_cboFs1_target = offLst + 23;
		IndexToFn.Add (s_cboFs1_target, "cboFs1_target"); //+23
		s_cboFs2_start = offLst + 24;
		IndexToFn.Add (s_cboFs2_start, "cboFs2_start"); //+24
		s_cboFs2_target = offLst + 25;
		IndexToFn.Add (s_cboFs2_target, "cboFs2_target"); //+25
		s_cboFs3_start = offLst + 26;
		IndexToFn.Add (s_cboFs3_start, "cboFs3_start"); //+26
		s_cboFs3_target = offLst + 27;
		IndexToFn.Add (s_cboFs3_target, "cboFs3_target"); //+27
		s_cboFs4_start = offLst + 28;
		IndexToFn.Add (s_cboFs4_start, "cboFs4_start"); //+28
		s_cboFs4_target = offLst + 29;
		IndexToFn.Add (s_cboFs4_target, "cboFs4_target"); //+29

		s_cboDs1_start = offLst + 30;
		IndexToFn.Add (s_cboDs1_start, "cboDs1_start"); //+30
		s_cboDs1_target = offLst + 31;
		IndexToFn.Add (s_cboDs1_target, "cboDs1_target"); //+31
		s_cboDs2_start = offLst + 32;
		IndexToFn.Add (s_cboDs2_start, "cboDs2_start"); //+32
		s_cboDs2_target = offLst + 33;
		IndexToFn.Add (s_cboDs2_target, "cboDs2_target"); //+33
		s_cboDs3_start = offLst + 34;
		IndexToFn.Add (s_cboDs3_start, "cboDs3_start"); //+34
		s_cboDs3_target = offLst + 35;
		IndexToFn.Add (s_cboDs3_target, "cboDs3_target"); //+35
		s_cboDs4_start = offLst + 36;
		IndexToFn.Add (s_cboDs4_start, "cboDs4_start"); //+36
		s_cboDs4_target = offLst + 37;
		IndexToFn.Add (s_cboDs4_target, "cboDs4_target"); //+37

		s_bmg_intro = offLst + 38;
		IndexToFn.Add (s_bmg_intro, "log-in-long2"); // +38
		s_bmg_select = offLst + 39;
		IndexToFn.Add (s_bmg_select, "sellect-loop2"); // +39
		s_bmg_field = offLst + 40;
		IndexToFn.Add (s_bmg_field, "Field2"); // +40
		s_bmg_gameover = offLst + 41;
		IndexToFn.Add (s_bmg_gameover, "game over2"); // +41
	}

	#region 声音常量

	public static int s_walk_ground_l = 1;
	public static int s_walk_ground_r = 2;
	public static int s_run_ground_l = 3;
	public static int s_run_ground_r = 4;
	public static int s_walk_stone_l = 5;
	public static int s_walk_stone_r = 6;
	public static int s_run_stone_l = 7;
	public static int s_run_stone_r = 8;
	public static int s_walk_lawn_l = 9;
	public static int s_walk_lawn_r = 10;
	public static int s_run_lawn_l = 11;
	public static int s_run_lawn_r = 12;
	public static int s_walk_rough_l = 13;
	public static int s_walk_rough_r = 14;
	public static int s_run_rough_l = 15;
	public static int s_run_rough_r = 16;
	public static int s_walk_wood_l = 17;
	public static int s_walk_wood_r = 18;
	public static int s_run_wood_l = 19;
	public static int s_run_wood_r = 20;
	public static int s_walk_cave_l = 21;
	public static int s_walk_cave_r = 22;
	public static int s_run_cave_l = 23;
	public static int s_run_cave_r = 24;
	public static int s_walk_room_l = 25;
	public static int s_walk_room_r = 26;
	public static int s_run_room_l = 27;
	public static int s_run_room_r = 28;
	public static int s_walk_water_l = 29;
	public static int s_walk_water_r = 30;
	public static int s_run_water_l = 31;
	public static int s_run_water_r = 32;

	public static int s_hit_short = 50;
	public static int s_hit_wooden = 51;
	public static int s_hit_sword = 52;
	public static int s_hit_do = 53;
	public static int s_hit_axe = 54;
	public static int s_hit_club = 55;
	public static int s_hit_long = 56;
	public static int s_hit_fist = 57;

	public static int s_struck_short = 60;
	public static int s_struck_wooden = 61;
	public static int s_struck_sword = 62;
	public static int s_struck_do = 63;
	public static int s_struck_axe = 64;
	public static int s_struck_club = 65;
	public static int s_struck_body_sword = 70;
	public static int s_struck_body_axe = 71;
	public static int s_struck_body_longstick = 72;
	public static int s_struck_body_fist = 73;
	public static int s_struck_armor_sword = 80;
	public static int s_struck_armor_axe = 81;
	public static int s_struck_armor_longstick = 82;
	public static int s_struck_armor_fist = 83;

	//public static int s_powerup_man         = 80;
	//public static int s_powerup_woman       = 81;
	//public static int s_die_man             = 82;
	//public static int s_die_woman           = 83;
	//public static int s_struck_man          = 84;
	//public static int s_struck_woman        = 85;
	//public static int s_firehit             = 86;
	//public static int s_struck_magic        = 90;
	public static int s_strike_stone = 91;
	public static int s_drop_stonepiece = 92;
	public static int s_rock_door_open = 100;
	public static int s_intro_theme = 102;
	public static int s_meltstone = 101;
	public static int s_main_theme = 102;
	public static int s_norm_button_click = 103;
	public static int s_rock_button_click = 104;
	public static int s_glass_button_click = 105;
	public static int s_money = 106;
	public static int s_eat_drug = 107;
	public static int s_click_drug = 108;
	public static int s_spacemove_out = 109;
	public static int s_spacemove_in = 110;
	public static int s_click_weapon = 111;
	public static int s_click_armor = 112;
	public static int s_click_ring = 113;
	public static int s_click_armring = 114;
	public static int s_click_necklace = 115;
	public static int s_click_helmet = 116;
	public static int s_click_grobes = 117;
	public static int s_itmclick = 118;
	public static int s_yedo_man = 130;
	public static int s_yedo_woman = 131;
	public static int s_longhit = 132;
	public static int s_widehit = 133;
	public static int s_rush_l = 134;
	public static int s_rush_r = 135;
	public static int s_firehit_ready = 136;
	public static int s_firehit = 137;
	public static int s_man_struck = 138;
	public static int s_wom_struck = 139;
	public static int s_man_die = 144;
	public static int s_wom_die = 145;

	public static int s_FireFlower_1 = -1;
	public static int s_FireFlower_2 = -1;
	public static int s_FireFlower_3 = -1;
	public static int s_HeroLogIn = -1;
	public static int s_HeroLogOut = -1;
	public static int s_S11 = -1;
	public static int s_S12 = -1;
	public static int s_S13 = -1;
	public static int s_hero_shield = -1;
	public static int s_SelectBoxFlash = -1;
	public static int s_Flashbox = -1;
	public static int s_Openbox = -1;
	public static int s_powerup = -1;
	public static int s_hit_ZRJF_M = -1;
	public static int s_hit_ZRJF_w = -1;
	/*public static int s_cboZs_SJS_M = -1;
public static int s_cboZs_SJS_w = -1;
public static int s_cboZs_ZXC = -1;
public static int s_cboZs_DYZ_M = -1;
public static int s_cboZs_DYZ_w = -1;
public static int s_cboZs_HSQJ = -1;*/
	public static int s_cboZs1_start_m = -1;
	public static int s_cboZs1_start_w = -1;
	public static int s_cboZs2_start = -1;
	public static int s_cboZs3_start_m = -1;
	public static int s_cboZs3_start_w = -1;
	public static int s_cboZs4_start = -1;
	public static int s_cboFs1_start = -1;
	public static int s_cboFs1_target = -1;
	public static int s_cboFs2_start = -1;
	public static int s_cboFs2_target = -1;
	public static int s_cboFs3_start = -1;
	public static int s_cboFs3_target = -1;
	public static int s_cboFs4_start = -1;
	public static int s_cboFs4_target = -1;
	public static int s_cboDs1_start = -1;
	public static int s_cboDs1_target = -1;
	public static int s_cboDs2_start = -1;
	public static int s_cboDs2_target = -1;
	public static int s_cboDs3_start = -1;
	public static int s_cboDs3_target = -1;
	public static int s_cboDs4_start = -1;
	public static int s_cboDs4_target = -1;
	public static int s_bmg_intro = -1;
	public static int s_bmg_select = -1;
	public static int s_bmg_field = -1;
	public static int s_bmg_gameover = -1;

	#endregion 声音常量
}