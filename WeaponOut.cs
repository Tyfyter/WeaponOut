﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

/*
 RARITY:
 * 1 - Special/Unique early armour + weapons
 * 2 - Dungeon loot and other decent mid-early things
 * 3 - Hell/Jungle
 * 4 - Hardmode ore stuff
 * 5 - Mechanical Bosses
 * 6 - Unique/Powerful (intermediate)
 * 7 - Chloropyhte and Plantera Loot
 * 8 - Golem Loot, the Terrarian
 * 9 - Lunar materials, dev loot
 * 10 - Ancient Manipulator + Moonlord
 */

namespace WeaponOut
{
    public class WeaponOut : Mod
    {
        public static Texture2D textureSPSH;
        public static Texture2D textureDMNB;
        public static Texture2D textureMANB;

        public WeaponOut()
        {
            Properties = new ModProperties()
            {
                Autoload = true
                //AutoloadGores = true,
                //AutoloadSounds = true
            };
        }

        public override void PostSetupContent()
        {
            if (Main.netMode != 2)
            {
                textureDMNB = GetTexture("Projectiles/DemonBlast");
                textureMANB = GetTexture("Projectiles/ManaBlast");
                textureSPSH = GetTexture("Projectiles/SplinterShot");
            }
            else
            {
                Console.WriteLine("WeaponOut loaded with no errors:   net#4");
            }
        }
    }
}
