﻿using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace WeaponOut.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class Pavise : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fossil Shield"); //Ceratopsian Shield
            DisplayName.AddTranslation(GameCulture.Chinese, "化石盾牌");
            DisplayName.AddTranslation(GameCulture.Russian, "Костяной Щит");

            Tooltip.SetDefault(
                "10 defense when facing attacks\n" +
                "Grants immunity to knockback when facing attacks");
            Tooltip.AddTranslation(GameCulture.Chinese, "正面抵抗攻击时增加10防御且免疫击退");
            Tooltip.AddTranslation(GameCulture.Russian,
                "+10 защиты от тыловых атак\n" +
                "Защита от тылового отбрасывания");
        }
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.rare = 1;
            item.accessory = true;
            item.value = Item.sellPrice(0, 0, 20, 0);
        }
        public override void AddRecipes() {
            if (!ModConf.EnableAccessories) return;
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FossilOre, 25);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PlayerFX modPlayer = player.GetModPlayer<PlayerFX>();
            modPlayer.FrontDefence += 10;
            modPlayer.frontNoKnockback = true;
        }
    }
}
