﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace WeaponOut.Items.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class FistMartialHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Apprentice Headband");
            DisplayName.AddTranslation(GameCulture.Chinese, "学徒头带");
            DisplayName.AddTranslation(GameCulture.Russian, "Повязка Новичка");

            Tooltip.SetDefault("3% increased melee critical strike chance\n"
                + "Fighting bosses slowly empowers next melee attack, up to 400%");
            Tooltip.AddTranslation(GameCulture.Chinese, "增加3%近战暴击率\n与Boss战斗时，近战伤害会迅速提升\n最高为武器本身伤害的400%，击中敌人后重新计算");
			Tooltip.AddTranslation(GameCulture.Russian,
				"+3% шанс критического удара в ближнем бою\n"
                + "В битве с боссами следующая ближняя атака усиливается вплоть до 400%");


            ModTranslation text;
            
            text = mod.CreateTranslation("FistMartialHeadPower");
            text.SetDefault("Negates fall damage"); // ItemTooltip.LuckyHorseshoe
            text.AddTranslation(GameCulture.Chinese, "免疫坠落伤害");
            text.AddTranslation(GameCulture.Russian, "Поглощает урон от падения");
            mod.AddTranslation(text);
            
            text = mod.CreateTranslation("FistMartialHeadDefence");
            text.SetDefault("Increases length of invincibility after taking damage"); // ItemTooltip.CrossNecklace
            text.AddTranslation(GameCulture.Chinese, "受到伤害后的无敌时间增长");
            text.AddTranslation(GameCulture.Russian, "Продлевает время неуязвимости после получения урона");
            mod.AddTranslation(text);
            
            text = mod.CreateTranslation("FistMartialHeadSpeed");
            text.SetDefault("Increases running speed by 10 mph");
            text.AddTranslation(GameCulture.Chinese, "移动速度增加10mph");
            text.AddTranslation(GameCulture.Russian, "+10 единиц скорости");
            mod.AddTranslation(text);
            
            text = mod.CreateTranslation("FistMartialHeadGi");
            text.SetDefault("50% increased uppercut and divekick damage");
            text.AddTranslation(GameCulture.Chinese, "增加50%上勾拳和下踢伤害");
            text.AddTranslation(GameCulture.Russian, "+50% урон в прыжке и падении");
            mod.AddTranslation(text);
        }
        public override void SetDefaults()
        {
            item.defense = 1;
            item.value = Item.sellPrice(0, 0, 10, 0);

            item.width = 18;
            item.height = 18;
        }
        public override void AddRecipes() {
            if (!ModConf.EnableFists) return;
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeCrit += 3;
            player.GetModPlayer<PlayerFX>().patienceDamage = 4f; // Can do up to 400%
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }

        private byte armourSet = 0;
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            armourSet = 0;
            if (body.type == ModContent.ItemType<FistPowerBody>() &&
                legs.type == ModContent.ItemType<FistPowerLegs>())
            {
                armourSet = 1;
                return true;
            }
            else if (body.type == ModContent.ItemType<FistDefBody>() &&
                legs.type == ModContent.ItemType<FistDefLegs>())
            {
                armourSet = 2;
                return true;
            }
            else if (body.type == ModContent.ItemType<FistSpeedBody>() &&
                legs.type == ModContent.ItemType<FistSpeedLegs>())
            {
                armourSet = 3;
                return true;
            }
            else if (body.type == ItemID.Gi)
            {
                armourSet = 4;
                return true;
            }
            return false;
        }
        
        public override void UpdateArmorSet(Player player)
        {
            ModPlayerFists mpf = ModPlayerFists.Get(player);
            switch (armourSet)
            {
                case 1:
                    player.setBonus = WeaponOut.GetTranslationTextValue("FistMartialHeadPower");
                    player.noFallDmg = true;
                    break;
                case 2:
                    player.setBonus = WeaponOut.GetTranslationTextValue("FistMartialHeadDefence");
                    player.longInvince = true;
                    break;
                case 3:
                    player.setBonus = WeaponOut.GetTranslationTextValue("FistMartialHeadSpeed");
                    player.accRunSpeed += 2f;
                    break;
                case 4:
                    player.setBonus = WeaponOut.GetTranslationTextValue("FistMartialHeadGi");
                    mpf.uppercutDamage += 0.5f;
                    mpf.divekickDamage += 0.5f;
                    break;
            }
        }
    }
}