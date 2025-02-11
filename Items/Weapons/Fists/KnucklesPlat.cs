﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace WeaponOut.Items.Weapons.Fists
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class KnucklesPlat : ModItem
    {
        public static int comboEffect = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Platinum Knuckleduster");
            DisplayName.AddTranslation(GameCulture.Chinese, "铂金指虎");
            DisplayName.AddTranslation(GameCulture.Russian, "Платиновый Кастет");

            Tooltip.SetDefault(
                "<right> consumes combo to unleash spirit energy\n" +
                "Combo grants 7 bonus damage");
            Tooltip.AddTranslation(GameCulture.Chinese, "鼠标右键消耗连击能量以释放精神能量\n连击将奖励增加7点伤害");
            Tooltip.AddTranslation(GameCulture.Russian,
				"<right>, чтобы использовать комбо и выпустить духовную энергию\n" +
				"Комбо: +7 урон");

            comboEffect = ModPlayerFists.RegisterComboEffectID(ComboEffects);
        }
        public override void SetDefaults()
        {
            item.melee = true;
            item.damage = 22;
            item.useAnimation = 24; // Combos can increase speed by 30-50% since it halves remaining attack time
            item.knockBack = 4.5f;
            item.tileBoost = 5; // For fists, we read this as the combo power

            item.useTime = item.useAnimation * 2;
            item.shoot = ModContent.ProjectileType<Projectiles.SpiritBlast>();
            item.shootSpeed = 12f;

            item.value = Item.sellPrice(0, 0, 18, 0); // One bar

            item.UseSound = SoundID.Item7;
            item.useStyle = ModPlayerFists.useStyle;
            item.autoReuse = true;
            item.width = 20;
            item.height = 20;
        }
        public override void AddRecipes() {            if (!ModConf.EnableFists) return;
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumBar, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        { ModPlayerFists.ModifyTooltips(tooltips, item); }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 0)
            {   // Short dash brings up to max default speed.
                player.GetModPlayer<ModPlayerFists>().
                SetDashOnMovement(6f, 4f, 0.992f, 0.96f, true, 0);
            }
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return player.GetModPlayer<ModPlayerFists>().
                AltFunctionCombo(player, comboEffect);
        }
        /// <summary> The method called during a combo. Use for ongoing dust and gore effects. </summary>
        public static void ComboEffects(Player player, Item item, bool initial)
        {
            if (initial)
            {
                player.itemAnimation = player.itemAnimationMax + 20;
                Main.PlaySound(SoundID.DD2_SkyDragonsFurySwing, player.position);
            }
            // Charging
            Rectangle r = ModPlayerFists.UseItemGraphicbox(player, 16, 45);
            if (player.itemAnimation > player.itemAnimationMax)
            {
                // Charge effect
                Dust d = Main.dust[Dust.NewDust(r.TopLeft(), 16, 16, DustID.t_Martian, 0, 0, 100, default(Color), 1.2f)];
                d.position -= d.velocity * 10f;
                d.velocity /= 2;
                d.noGravity = true;
            }
            // Initial throw
            else if (player.itemAnimation == player.itemAnimationMax)
            {
                // Higher pitch
                Main.PlaySound(42, (int)player.position.X, (int)player.position.Y, 184, 1f, 0.5f);
                player.itemTime = 0;
            }
            else
            {
                // Punch effect
                Dust d = Main.dust[Dust.NewDust(r.TopLeft(), 16, 16, DustID.t_Martian, 3, 3, 100, default(Color), 1f)];
                d.velocity *= 0.6f * ModPlayerFists.GetFistVelocity(player);
                d.noGravity = true;
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.GetModPlayer<ModPlayerFists>().ComboEffectAbs == comboEffect &&
                player.itemAnimation < player.itemAnimationMax)
            {
                return true;
            }
            return false;
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            ModPlayerFists mpf = player.GetModPlayer<ModPlayerFists>();
            int size = 18;
            float jump = 10.5f;
            if (mpf.ComboEffectAbs == comboEffect)
            {
                size = (int)(size * 2.5f);
                jump = 11f;
            }
            ModPlayerFists.UseItemHitbox(player, ref hitbox, size, jump, 0.5f, 8f);
        }

        //Combo
        public override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
        { float knockBack = 5f; ModifyHit(player, ref damage, ref knockBack, ref crit); }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        { ModifyHit(player, ref damage, ref knockBack, ref crit); }
        private void ModifyHit(Player player, ref int damage, ref float knockBack, ref bool crit)
        {
            ModPlayerFists mpf = player.GetModPlayer<ModPlayerFists>();
            if (mpf.IsComboActiveItemOnHit)
            {
                damage += 7;
            }
            if(mpf.ComboEffectAbs == comboEffect)
            {
                damage += player.HeldItem.damage;
                knockBack *= 2f;
            }
        }
    }
}
