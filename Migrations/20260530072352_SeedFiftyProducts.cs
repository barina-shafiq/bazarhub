using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebProject.Migrations
{
    /// <inheritdoc />
    public partial class SeedFiftyProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CategoryId", "CreatedAt", "Description", "DiscountPrice", "IsFeatured", "IsTrending", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 6, "Nova", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Premium flagship with 200MP camera, 120Hz display, and 512GB storage.", 124999m, true, false, "Nova Pro Max 5G", 135000m, 15 },
                    { 7, "Apex", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Innovative folding design with flexible AMOLED screen and 12GB RAM.", 229999m, false, true, "Apex Fold Z", 245000m, 8 },
                    { 8, "Zeta", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Affordable daily driver with 5000mAh battery and clean Android OS.", 29999m, false, false, "Zeta Lite 4G", 32000m, 50 },
                    { 9, "Vibe", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Designed for productivity with integrated stylus and split-screen multitasking.", 41999m, false, false, "Vibe Note 10", 45000m, 30 },
                    { 10, "Google", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Exceptional camera software, pure Google UI, and fast system updates.", null, true, false, "PixelPro X", 165000m, 10 },
                    { 11, "Anker", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Fast wireless charging pad compatible with all Qi-enabled devices.", 3999m, false, false, "ChargePad Pro 15W", 4500m, 120 },
                    { 12, "Armor", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Ultra-tough 9H tempered glass screen protector with oleophobic coating.", null, false, false, "Armor Glass Screen Guard", 1200m, 200 },
                    { 13, "Aero", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Crystal clear shockproof protective cover that highlights your phone.", null, false, false, "AeroCase Slim", 1800m, 150 },
                    { 14, "Anker", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "High capacity portable charger with dual USB outputs and PowerIQ.", 7499m, false, true, "PowerCore 20k Powerbank", 8500m, 90 },
                    { 15, "Studio", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "10-inch LED ring light with tripod stand and phone holder for streaming.", 2999m, false, false, "RingLight Stand v2", 3500m, 45 },
                    { 16, "Vibe", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Extendable bluetooth selfie stick with remote shutter control.", null, false, false, "SelfieStick Mono", 2200m, 75 },
                    { 17, "Studio", 1, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Flexible octopus-style mini tripod for stable multi-angle shots.", 2499m, false, false, "Tripod Flexi", 2800m, 60 },
                    { 18, "Zen", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "High-performance laptop with 16-inch OLED screen, RTX GPU, and 32GB RAM.", 269999m, true, false, "ZenBook Creator Pro", 285000m, 5 },
                    { 19, "Swift", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Compact student laptop with long battery life and lightweight design.", 41999m, false, false, "SwiftBook Lite 11", 45000m, 25 },
                    { 20, "Pulse", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Premium active noise cancelling wireless headphones with 40h runtime.", 16999m, true, false, "Pulse Over-Ear ANC", 18500m, 35 },
                    { 21, "Vibe", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Rich home theatre soundbar with bluetooth, HDMI ARC, and deep bass.", 21999m, false, false, "Vibe Soundbar 120W", 24500m, 18 },
                    { 22, "Swift", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "11-inch Android tablet with responsive active stylus and 2K display.", 59999m, false, true, "SwiftPad Pro Tablet", 65000m, 22 },
                    { 23, "Pulse", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Fitness tracker with heart rate, blood oxygen, sleep analytics, and GPS.", 10999m, false, true, "SmartWatch Active 4", 12500m, 65 },
                    { 24, "Aero", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Standalone virtual reality headset with dual controllers and spatial audio.", 68999m, false, false, "AeroVR Headset", 75000m, 10 },
                    { 25, "Nova", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Premium wireless earbuds with crystal clear calls and sweat resistance.", 12999m, false, false, "AirBuds Pro ANC", 14500m, 85 },
                    { 26, "Pulse", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Secure-fit ear hooks, powerful sound, and IPX7 rating for running.", 5499m, false, false, "Pulse Sport Earbuds", 6500m, 110 },
                    { 27, "Pulse", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Ultra-portable waterproof bluetooth speaker with rich 360-degree sound.", 3999m, false, false, "SoundBox Mini Speaker", 4800m, 140 },
                    { 28, "Studio", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "1080p stream webcam with dual microphones and autoframing.", 8499m, false, false, "HD Webcam Streamer", 9500m, 50 },
                    { 29, "Gaming", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Tactile mechanical keyboard with customized RGB backlighting.", 9999m, false, false, "Mechanical Keyboard Blue", 11500m, 40 },
                    { 30, "Gaming", 2, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "High-precision ergonomic mouse that reduces wrist strain during work.", 3999m, false, false, "Ergonomic Wireless Mouse", 4500m, 95 },
                    { 31, "Streetly", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Stretchable cotton trousers that perfectly balance smart and casual.", 3200m, false, false, "Slim Fit Cotton Chinos", 3800m, 40 },
                    { 32, "Streetly", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "100% genuine full-grain leather belt with polished silver buckle.", 1899m, false, false, "Classic Leather Belt", 2200m, 90 },
                    { 33, "Streetly", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Cozy pullover hoodie lined with soft premium fleece.", 4999m, false, true, "Urban Hoodie Sweatshirt", 5500m, 30 },
                    { 34, "Vogue", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Lightweight breathable sundress featuring a stylish modern floral print.", 6500m, true, false, "Summer Floral Dress", 7500m, 25 },
                    { 35, "Vogue", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Tailored slim-fit blazer that instantly smartens any outfit.", 10999m, false, false, "Formal Blazer Classic", 12500m, 15 },
                    { 36, "Runners", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "High-traction running shoes with cushioned responsive midsole.", 8200m, false, false, "Running Shoes Swift", 9500m, 50 },
                    { 37, "Runners", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Timeless suede leather chelsea boots with elastic side panels.", 12999m, true, false, "Leather Chelsea Boots", 14500m, 20 },
                    { 38, "Runners", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Classic high-top canvas sneakers suitable for everyday wear.", 4999m, false, false, "Canvas High-Top Sneakers", 5800m, 70 },
                    { 39, "Vogue", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Elegant sleek watch with black dial and silver stainless steel strap.", 7500m, false, true, "Minimalist Quartz Watch", 8500m, 35 },
                    { 40, "Aero", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "UV400 protective polarized sunglasses with lightweight frame.", 2999m, false, false, "Aero Polarized Sunglasses", 3500m, 110 },
                    { 41, "Streetly", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Cool linen-blend casual shirt with classic button-down collar.", 3600m, false, false, "Casual Linen Shirt", 4200m, 55 },
                    { 42, "Vogue", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Sleek textured leather handbag with gold-finished hardware accents.", 18999m, false, false, "Designer Handbag Elegance", 22000m, 12 },
                    { 43, "Streetly", 3, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Bifold design leather wallet with multiple card slots and RFID protection.", null, false, false, "Compact Leather Wallet", 2800m, 80 },
                    { 44, "ChefMate", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Large 5.5L capacity air fryer with digital presets and rapid air technology.", 23999m, true, false, "ChefMate Air Fryer XL", 26500m, 15 },
                    { 45, "ChefMate", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "12-cup automatic coffee maker with programmable timer and hot plate.", 7499m, false, false, "Drip Coffee Maker", 8500m, 40 },
                    { 46, "ChefMate", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "10-piece aluminum non-stick cookware set including pots, pans, and lids.", 16999m, false, false, "Non-Stick Cookware Set", 18500m, 18 },
                    { 47, "ChefMate", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "High-precision scale that measures up to 5kg in ounces, grams, and ml.", 1850m, false, false, "Digital Kitchen Scale", 2200m, 120 },
                    { 48, "Glow", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "LED desk lamp with adjustable brightness, touch control, and USB port.", 3800m, false, false, "Ergonomic Desk Lamp", 4500m, 60 },
                    { 49, "Glow", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Quiet aromatherapy oil diffuser with color-changing ambient lighting.", 2799m, false, false, "Diffuser Ultrasonic", 3200m, 85 },
                    { 50, "Rest", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Contoured memory foam pillow that provides neck support for all sleepers.", 3999m, false, false, "Memory Foam Pillow", 4800m, 50 },
                    { 51, "Rest", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "100% long-staple cotton bedsheet with two matching pillowcases.", 5499m, false, true, "Cotton Bedsheet King", 6500m, 35 },
                    { 52, "ChefMate", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Vertical upright clothing steamer with removable 1.5L tank and hanger.", 8500m, false, false, "Heavy Duty Garment Steamer", 9800m, 22 },
                    { 53, "Glow", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Smart app-controlled RGB LED strip light that syncs with music.", 2999m, false, false, "Smart LED Strip 5m", 3500m, 90 },
                    { 54, "ChefMate", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Double-walled vacuum insulated thermal bottle that stays hot for 24h.", 2400m, false, false, "Stainless Steel Flask 1L", 2800m, 110 },
                    { 55, "ChefMate", 4, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Touchless motion-sensor soap and hand sanitizer dispenser.", null, false, false, "Automatic Hand Sanitizer", 3200m, 130 }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImagePath", "IsPrimary", "ProductId" },
                values: new object[,]
                {
                    { 6, "/uploads/sample-phone.svg", true, 6 },
                    { 7, "/uploads/sample-phone.svg", true, 7 },
                    { 8, "/uploads/sample-phone.svg", true, 8 },
                    { 9, "/uploads/sample-phone.svg", true, 9 },
                    { 10, "/uploads/sample-phone.svg", true, 10 },
                    { 11, "/uploads/sample-phone.svg", true, 11 },
                    { 12, "/uploads/sample-phone.svg", true, 12 },
                    { 13, "/uploads/sample-phone.svg", true, 13 },
                    { 14, "/uploads/sample-phone.svg", true, 14 },
                    { 15, "/uploads/sample-phone.svg", true, 15 },
                    { 16, "/uploads/sample-phone.svg", true, 16 },
                    { 17, "/uploads/sample-phone.svg", true, 17 },
                    { 18, "/uploads/sample-laptop.svg", true, 18 },
                    { 19, "/uploads/sample-laptop.svg", true, 19 },
                    { 20, "/uploads/sample-laptop.svg", true, 20 },
                    { 21, "/uploads/sample-laptop.svg", true, 21 },
                    { 22, "/uploads/sample-laptop.svg", true, 22 },
                    { 23, "/uploads/sample-laptop.svg", true, 23 },
                    { 24, "/uploads/sample-laptop.svg", true, 24 },
                    { 25, "/uploads/sample-earbuds.svg", true, 25 },
                    { 26, "/uploads/sample-earbuds.svg", true, 26 },
                    { 27, "/uploads/sample-earbuds.svg", true, 27 },
                    { 28, "/uploads/sample-earbuds.svg", true, 28 },
                    { 29, "/uploads/sample-earbuds.svg", true, 29 },
                    { 30, "/uploads/sample-earbuds.svg", true, 30 },
                    { 31, "/uploads/sample-fashion.svg", true, 31 },
                    { 32, "/uploads/sample-fashion.svg", true, 32 },
                    { 33, "/uploads/sample-fashion.svg", true, 33 },
                    { 34, "/uploads/sample-fashion.svg", true, 34 },
                    { 35, "/uploads/sample-fashion.svg", true, 35 },
                    { 36, "/uploads/sample-fashion.svg", true, 36 },
                    { 37, "/uploads/sample-fashion.svg", true, 37 },
                    { 38, "/uploads/sample-fashion.svg", true, 38 },
                    { 39, "/uploads/sample-fashion.svg", true, 39 },
                    { 40, "/uploads/sample-fashion.svg", true, 40 },
                    { 41, "/uploads/sample-fashion.svg", true, 41 },
                    { 42, "/uploads/sample-fashion.svg", true, 42 },
                    { 43, "/uploads/sample-fashion.svg", true, 43 },
                    { 44, "/uploads/sample-home.svg", true, 44 },
                    { 45, "/uploads/sample-home.svg", true, 45 },
                    { 46, "/uploads/sample-home.svg", true, 46 },
                    { 47, "/uploads/sample-home.svg", true, 47 },
                    { 48, "/uploads/sample-home.svg", true, 48 },
                    { 49, "/uploads/sample-home.svg", true, 49 },
                    { 50, "/uploads/sample-home.svg", true, 50 },
                    { 51, "/uploads/sample-home.svg", true, 51 },
                    { 52, "/uploads/sample-home.svg", true, 52 },
                    { 53, "/uploads/sample-home.svg", true, 53 },
                    { 54, "/uploads/sample-home.svg", true, 54 },
                    { 55, "/uploads/sample-home.svg", true, 55 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 55);
        }
    }
}
