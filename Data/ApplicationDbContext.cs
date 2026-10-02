using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebProject.Models;

namespace WebProject.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Address> Addresses => Set<Address>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
        builder.Entity<Product>().Property(p => p.DiscountPrice).HasColumnType("decimal(18,2)");
        builder.Entity<Order>().Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
        builder.Entity<Order>().Property(o => o.ShippingFee).HasColumnType("decimal(18,2)");
        builder.Entity<Order>().Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Entity<OrderItem>().Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Entity<OrderItem>().Property(i => i.LineTotal).HasColumnType("decimal(18,2)");

        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Cart>()
            .HasIndex(c => c.UserId)
            .IsUnique();

        builder.Entity<Wishlist>()
            .HasIndex(w => w.UserId)
            .IsUnique();

        builder.Entity<CartItem>()
            .HasIndex(i => new { i.CartId, i.ProductId })
            .IsUnique();

        builder.Entity<WishlistItem>()
            .HasIndex(i => new { i.WishlistId, i.ProductId })
            .IsUnique();

        builder.Entity<Review>()
            .HasIndex(r => new { r.ProductId, r.UserId })
            .IsUnique();

        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.Address)
            .WithMany()
            .HasForeignKey(o => o.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        var seedDate = new DateTime(2026, 5, 25, 0, 0, 0, DateTimeKind.Utc);

        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Mobiles", Description = "Smartphones and accessories" },
            new Category { Id = 2, Name = "Electronics", Description = "Laptops, audio, and gadgets" },
            new Category { Id = 3, Name = "Fashion", Description = "Clothing and lifestyle products" },
            new Category { Id = 4, Name = "Home", Description = "Home and kitchen essentials" });

        builder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Nova X1 Smartphone", Brand = "Nova", Description = "AMOLED display, 5G, 128GB storage, and fast charging.", Price = 89999, DiscountPrice = 82999, StockQuantity = 25, CategoryId = 1, IsFeatured = true, IsTrending = true, CreatedAt = seedDate },
            new Product { Id = 2, Name = "SwiftBook Air 14", Brand = "Swift", Description = "Lightweight laptop for study, work, and entertainment.", Price = 185000, DiscountPrice = 174500, StockQuantity = 12, CategoryId = 2, IsFeatured = true, CreatedAt = seedDate },
            new Product { Id = 3, Name = "Pulse Wireless Earbuds", Brand = "Pulse", Description = "Noise isolation, touch controls, and compact charging case.", Price = 9500, DiscountPrice = 7999, StockQuantity = 80, CategoryId = 2, IsTrending = true, CreatedAt = seedDate },
            new Product { Id = 4, Name = "Urban Denim Jacket", Brand = "Streetly", Description = "Classic fit denim jacket with premium stitching.", Price = 6500, StockQuantity = 35, CategoryId = 3, IsFeatured = true, CreatedAt = seedDate },
            new Product { Id = 5, Name = "ChefMate Blender Pro", Brand = "ChefMate", Description = "Durable blender with stainless steel blades and pulse modes.", Price = 14500, DiscountPrice = 12999, StockQuantity = 20, CategoryId = 4, IsTrending = true, CreatedAt = seedDate },
            // Mobiles (Ids 6 - 17)
            new Product { Id = 6, Name = "Nova Pro Max 5G", Brand = "Nova", Description = "Premium flagship with 200MP camera, 120Hz display, and 512GB storage.", Price = 135000, DiscountPrice = 124999, StockQuantity = 15, CategoryId = 1, IsFeatured = true, CreatedAt = seedDate },
            new Product { Id = 7, Name = "Apex Fold Z", Brand = "Apex", Description = "Innovative folding design with flexible AMOLED screen and 12GB RAM.", Price = 245000, DiscountPrice = 229999, StockQuantity = 8, CategoryId = 1, IsTrending = true, CreatedAt = seedDate },
            new Product { Id = 8, Name = "Zeta Lite 4G", Brand = "Zeta", Description = "Affordable daily driver with 5000mAh battery and clean Android OS.", Price = 32000, DiscountPrice = 29999, StockQuantity = 50, CategoryId = 1, CreatedAt = seedDate },
            new Product { Id = 9, Name = "Vibe Note 10", Brand = "Vibe", Description = "Designed for productivity with integrated stylus and split-screen multitasking.", Price = 45000, DiscountPrice = 41999, StockQuantity = 30, CategoryId = 1, CreatedAt = seedDate },
            new Product { Id = 10, Name = "PixelPro X", Brand = "Google", Description = "Exceptional camera software, pure Google UI, and fast system updates.", Price = 165000, StockQuantity = 10, CategoryId = 1, IsFeatured = true, CreatedAt = seedDate },
            new Product { Id = 11, Name = "ChargePad Pro 15W", Brand = "Anker", Description = "Fast wireless charging pad compatible with all Qi-enabled devices.", Price = 4500, DiscountPrice = 3999, StockQuantity = 120, CategoryId = 1, CreatedAt = seedDate },
            new Product { Id = 12, Name = "Armor Glass Screen Guard", Brand = "Armor", Description = "Ultra-tough 9H tempered glass screen protector with oleophobic coating.", Price = 1200, StockQuantity = 200, CategoryId = 1, CreatedAt = seedDate },
            new Product { Id = 13, Name = "AeroCase Slim", Brand = "Aero", Description = "Crystal clear shockproof protective cover that highlights your phone.", Price = 1800, StockQuantity = 150, CategoryId = 1, CreatedAt = seedDate },
            new Product { Id = 14, Name = "PowerCore 20k Powerbank", Brand = "Anker", Description = "High capacity portable charger with dual USB outputs and PowerIQ.", Price = 8500, DiscountPrice = 7499, StockQuantity = 90, CategoryId = 1, IsTrending = true, CreatedAt = seedDate },
            new Product { Id = 15, Name = "RingLight Stand v2", Brand = "Studio", Description = "10-inch LED ring light with tripod stand and phone holder for streaming.", Price = 3500, DiscountPrice = 2999, StockQuantity = 45, CategoryId = 1, CreatedAt = seedDate },
            new Product { Id = 16, Name = "SelfieStick Mono", Brand = "Vibe", Description = "Extendable bluetooth selfie stick with remote shutter control.", Price = 2200, StockQuantity = 75, CategoryId = 1, CreatedAt = seedDate },
            new Product { Id = 17, Name = "Tripod Flexi", Brand = "Studio", Description = "Flexible octopus-style mini tripod for stable multi-angle shots.", Price = 2800, DiscountPrice = 2499, StockQuantity = 60, CategoryId = 1, CreatedAt = seedDate },
            // Electronics (Ids 18 - 30)
            new Product { Id = 18, Name = "ZenBook Creator Pro", Brand = "Zen", Description = "High-performance laptop with 16-inch OLED screen, RTX GPU, and 32GB RAM.", Price = 285000, DiscountPrice = 269999, StockQuantity = 5, CategoryId = 2, IsFeatured = true, CreatedAt = seedDate },
            new Product { Id = 19, Name = "SwiftBook Lite 11", Brand = "Swift", Description = "Compact student laptop with long battery life and lightweight design.", Price = 45000, DiscountPrice = 41999, StockQuantity = 25, CategoryId = 2, CreatedAt = seedDate },
            new Product { Id = 20, Name = "Pulse Over-Ear ANC", Brand = "Pulse", Description = "Premium active noise cancelling wireless headphones with 40h runtime.", Price = 18500, DiscountPrice = 16999, StockQuantity = 35, CategoryId = 2, IsFeatured = true, CreatedAt = seedDate },
            new Product { Id = 21, Name = "Vibe Soundbar 120W", Brand = "Vibe", Description = "Rich home theatre soundbar with bluetooth, HDMI ARC, and deep bass.", Price = 24500, DiscountPrice = 21999, StockQuantity = 18, CategoryId = 2, CreatedAt = seedDate },
            new Product { Id = 22, Name = "SwiftPad Pro Tablet", Brand = "Swift", Description = "11-inch Android tablet with responsive active stylus and 2K display.", Price = 65000, DiscountPrice = 59999, StockQuantity = 22, CategoryId = 2, IsTrending = true, CreatedAt = seedDate },
            new Product { Id = 23, Name = "SmartWatch Active 4", Brand = "Pulse", Description = "Fitness tracker with heart rate, blood oxygen, sleep analytics, and GPS.", Price = 12500, DiscountPrice = 10999, StockQuantity = 65, CategoryId = 2, IsTrending = true, CreatedAt = seedDate },
            new Product { Id = 24, Name = "AeroVR Headset", Brand = "Aero", Description = "Standalone virtual reality headset with dual controllers and spatial audio.", Price = 75000, DiscountPrice = 68999, StockQuantity = 10, CategoryId = 2, CreatedAt = seedDate },
            new Product { Id = 25, Name = "AirBuds Pro ANC", Brand = "Nova", Description = "Premium wireless earbuds with crystal clear calls and sweat resistance.", Price = 14500, DiscountPrice = 12999, StockQuantity = 85, CategoryId = 2, CreatedAt = seedDate },
            new Product { Id = 26, Name = "Pulse Sport Earbuds", Brand = "Pulse", Description = "Secure-fit ear hooks, powerful sound, and IPX7 rating for running.", Price = 6500, DiscountPrice = 5499, StockQuantity = 110, CategoryId = 2, CreatedAt = seedDate },
            new Product { Id = 27, Name = "SoundBox Mini Speaker", Brand = "Pulse", Description = "Ultra-portable waterproof bluetooth speaker with rich 360-degree sound.", Price = 4800, DiscountPrice = 3999, StockQuantity = 140, CategoryId = 2, CreatedAt = seedDate },
            new Product { Id = 28, Name = "HD Webcam Streamer", Brand = "Studio", Description = "1080p stream webcam with dual microphones and autoframing.", Price = 9500, DiscountPrice = 8499, StockQuantity = 50, CategoryId = 2, CreatedAt = seedDate },
            new Product { Id = 29, Name = "Mechanical Keyboard Blue", Brand = "Gaming", Description = "Tactile mechanical keyboard with customized RGB backlighting.", Price = 11500, DiscountPrice = 9999, StockQuantity = 40, CategoryId = 2, CreatedAt = seedDate },
            new Product { Id = 30, Name = "Ergonomic Wireless Mouse", Brand = "Gaming", Description = "High-precision ergonomic mouse that reduces wrist strain during work.", Price = 4500, DiscountPrice = 3999, StockQuantity = 95, CategoryId = 2, CreatedAt = seedDate },
            // Fashion (Ids 31 - 43)
            new Product { Id = 31, Name = "Slim Fit Cotton Chinos", Brand = "Streetly", Description = "Stretchable cotton trousers that perfectly balance smart and casual.", Price = 3800, DiscountPrice = 3200, StockQuantity = 40, CategoryId = 3, CreatedAt = seedDate },
            new Product { Id = 32, Name = "Classic Leather Belt", Brand = "Streetly", Description = "100% genuine full-grain leather belt with polished silver buckle.", Price = 2200, DiscountPrice = 1899, StockQuantity = 90, CategoryId = 3, CreatedAt = seedDate },
            new Product { Id = 33, Name = "Urban Hoodie Sweatshirt", Brand = "Streetly", Description = "Cozy pullover hoodie lined with soft premium fleece.", Price = 5500, DiscountPrice = 4999, StockQuantity = 30, CategoryId = 3, IsTrending = true, CreatedAt = seedDate },
            new Product { Id = 34, Name = "Summer Floral Dress", Brand = "Vogue", Description = "Lightweight breathable sundress featuring a stylish modern floral print.", Price = 7500, DiscountPrice = 6500, StockQuantity = 25, CategoryId = 3, IsFeatured = true, CreatedAt = seedDate },
            new Product { Id = 35, Name = "Formal Blazer Classic", Brand = "Vogue", Description = "Tailored slim-fit blazer that instantly smartens any outfit.", Price = 12500, DiscountPrice = 10999, StockQuantity = 15, CategoryId = 3, CreatedAt = seedDate },
            new Product { Id = 36, Name = "Running Shoes Swift", Brand = "Runners", Description = "High-traction running shoes with cushioned responsive midsole.", Price = 9500, DiscountPrice = 8200, StockQuantity = 50, CategoryId = 3, CreatedAt = seedDate },
            new Product { Id = 37, Name = "Leather Chelsea Boots", Brand = "Runners", Description = "Timeless suede leather chelsea boots with elastic side panels.", Price = 14500, DiscountPrice = 12999, StockQuantity = 20, CategoryId = 3, IsFeatured = true, CreatedAt = seedDate },
            new Product { Id = 38, Name = "Canvas High-Top Sneakers", Brand = "Runners", Description = "Classic high-top canvas sneakers suitable for everyday wear.", Price = 5800, DiscountPrice = 4999, StockQuantity = 70, CategoryId = 3, CreatedAt = seedDate },
            new Product { Id = 39, Name = "Minimalist Quartz Watch", Brand = "Vogue", Description = "Elegant sleek watch with black dial and silver stainless steel strap.", Price = 8500, DiscountPrice = 7500, StockQuantity = 35, CategoryId = 3, IsTrending = true, CreatedAt = seedDate },
            new Product { Id = 40, Name = "Aero Polarized Sunglasses", Brand = "Aero", Description = "UV400 protective polarized sunglasses with lightweight frame.", Price = 3500, DiscountPrice = 2999, StockQuantity = 110, CategoryId = 3, CreatedAt = seedDate },
            new Product { Id = 41, Name = "Casual Linen Shirt", Brand = "Streetly", Description = "Cool linen-blend casual shirt with classic button-down collar.", Price = 4200, DiscountPrice = 3600, StockQuantity = 55, CategoryId = 3, CreatedAt = seedDate },
            new Product { Id = 42, Name = "Designer Handbag Elegance", Brand = "Vogue", Description = "Sleek textured leather handbag with gold-finished hardware accents.", Price = 22000, DiscountPrice = 18999, StockQuantity = 12, CategoryId = 3, CreatedAt = seedDate },
            new Product { Id = 43, Name = "Compact Leather Wallet", Brand = "Streetly", Description = "Bifold design leather wallet with multiple card slots and RFID protection.", Price = 2800, StockQuantity = 80, CategoryId = 3, CreatedAt = seedDate },
            // Home (Ids 44 - 55)
            new Product { Id = 44, Name = "ChefMate Air Fryer XL", Brand = "ChefMate", Description = "Large 5.5L capacity air fryer with digital presets and rapid air technology.", Price = 26500, DiscountPrice = 23999, StockQuantity = 15, CategoryId = 4, IsFeatured = true, CreatedAt = seedDate },
            new Product { Id = 45, Name = "Drip Coffee Maker", Brand = "ChefMate", Description = "12-cup automatic coffee maker with programmable timer and hot plate.", Price = 8500, DiscountPrice = 7499, StockQuantity = 40, CategoryId = 4, CreatedAt = seedDate },
            new Product { Id = 46, Name = "Non-Stick Cookware Set", Brand = "ChefMate", Description = "10-piece aluminum non-stick cookware set including pots, pans, and lids.", Price = 18500, DiscountPrice = 16999, StockQuantity = 18, CategoryId = 4, CreatedAt = seedDate },
            new Product { Id = 47, Name = "Digital Kitchen Scale", Brand = "ChefMate", Description = "High-precision scale that measures up to 5kg in ounces, grams, and ml.", Price = 2200, DiscountPrice = 1850, StockQuantity = 120, CategoryId = 4, CreatedAt = seedDate },
            new Product { Id = 48, Name = "Ergonomic Desk Lamp", Brand = "Glow", Description = "LED desk lamp with adjustable brightness, touch control, and USB port.", Price = 4500, DiscountPrice = 3800, StockQuantity = 60, CategoryId = 4, CreatedAt = seedDate },
            new Product { Id = 49, Name = "Diffuser Ultrasonic", Brand = "Glow", Description = "Quiet aromatherapy oil diffuser with color-changing ambient lighting.", Price = 3200, DiscountPrice = 2799, StockQuantity = 85, CategoryId = 4, CreatedAt = seedDate },
            new Product { Id = 50, Name = "Memory Foam Pillow", Brand = "Rest", Description = "Contoured memory foam pillow that provides neck support for all sleepers.", Price = 4800, DiscountPrice = 3999, StockQuantity = 50, CategoryId = 4, CreatedAt = seedDate },
            new Product { Id = 51, Name = "Cotton Bedsheet King", Brand = "Rest", Description = "100% long-staple cotton bedsheet with two matching pillowcases.", Price = 6500, DiscountPrice = 5499, StockQuantity = 35, CategoryId = 4, IsTrending = true, CreatedAt = seedDate },
            new Product { Id = 52, Name = "Heavy Duty Garment Steamer", Brand = "ChefMate", Description = "Vertical upright clothing steamer with removable 1.5L tank and hanger.", Price = 9800, DiscountPrice = 8500, StockQuantity = 22, CategoryId = 4, CreatedAt = seedDate },
            new Product { Id = 53, Name = "Smart LED Strip 5m", Brand = "Glow", Description = "Smart app-controlled RGB LED strip light that syncs with music.", Price = 3500, DiscountPrice = 2999, StockQuantity = 90, CategoryId = 4, CreatedAt = seedDate },
            new Product { Id = 54, Name = "Stainless Steel Flask 1L", Brand = "ChefMate", Description = "Double-walled vacuum insulated thermal bottle that stays hot for 24h.", Price = 2800, DiscountPrice = 2400, StockQuantity = 110, CategoryId = 4, CreatedAt = seedDate },
            new Product { Id = 55, Name = "Automatic Hand Sanitizer", Brand = "ChefMate", Description = "Touchless motion-sensor soap and hand sanitizer dispenser.", Price = 3200, StockQuantity = 130, CategoryId = 4, CreatedAt = seedDate });

        builder.Entity<ProductImage>().HasData(
            new ProductImage { Id = 1, ProductId = 1, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 2, ProductId = 2, ImagePath = "/uploads/sample-laptop.svg", IsPrimary = true },
            new ProductImage { Id = 3, ProductId = 3, ImagePath = "/uploads/sample-earbuds.svg", IsPrimary = true },
            new ProductImage { Id = 4, ProductId = 4, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 5, ProductId = 5, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            // Mobiles (6 - 17)
            new ProductImage { Id = 6, ProductId = 6, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 7, ProductId = 7, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 8, ProductId = 8, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 9, ProductId = 9, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 10, ProductId = 10, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 11, ProductId = 11, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 12, ProductId = 12, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 13, ProductId = 13, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 14, ProductId = 14, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 15, ProductId = 15, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 16, ProductId = 16, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            new ProductImage { Id = 17, ProductId = 17, ImagePath = "/uploads/sample-phone.svg", IsPrimary = true },
            // Electronics (18 - 30)
            new ProductImage { Id = 18, ProductId = 18, ImagePath = "/uploads/sample-laptop.svg", IsPrimary = true },
            new ProductImage { Id = 19, ProductId = 19, ImagePath = "/uploads/sample-laptop.svg", IsPrimary = true },
            new ProductImage { Id = 20, ProductId = 20, ImagePath = "/uploads/sample-laptop.svg", IsPrimary = true },
            new ProductImage { Id = 21, ProductId = 21, ImagePath = "/uploads/sample-laptop.svg", IsPrimary = true },
            new ProductImage { Id = 22, ProductId = 22, ImagePath = "/uploads/sample-laptop.svg", IsPrimary = true },
            new ProductImage { Id = 23, ProductId = 23, ImagePath = "/uploads/sample-laptop.svg", IsPrimary = true },
            new ProductImage { Id = 24, ProductId = 24, ImagePath = "/uploads/sample-laptop.svg", IsPrimary = true },
            new ProductImage { Id = 25, ProductId = 25, ImagePath = "/uploads/sample-earbuds.svg", IsPrimary = true },
            new ProductImage { Id = 26, ProductId = 26, ImagePath = "/uploads/sample-earbuds.svg", IsPrimary = true },
            new ProductImage { Id = 27, ProductId = 27, ImagePath = "/uploads/sample-earbuds.svg", IsPrimary = true },
            new ProductImage { Id = 28, ProductId = 28, ImagePath = "/uploads/sample-earbuds.svg", IsPrimary = true },
            new ProductImage { Id = 29, ProductId = 29, ImagePath = "/uploads/sample-earbuds.svg", IsPrimary = true },
            new ProductImage { Id = 30, ProductId = 30, ImagePath = "/uploads/sample-earbuds.svg", IsPrimary = true },
            // Fashion (31 - 43)
            new ProductImage { Id = 31, ProductId = 31, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 32, ProductId = 32, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 33, ProductId = 33, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 34, ProductId = 34, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 35, ProductId = 35, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 36, ProductId = 36, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 37, ProductId = 37, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 38, ProductId = 38, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 39, ProductId = 39, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 40, ProductId = 40, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 41, ProductId = 41, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 42, ProductId = 42, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            new ProductImage { Id = 43, ProductId = 43, ImagePath = "/uploads/sample-fashion.svg", IsPrimary = true },
            // Home (44 - 55)
            new ProductImage { Id = 44, ProductId = 44, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 45, ProductId = 45, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 46, ProductId = 46, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 47, ProductId = 47, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 48, ProductId = 48, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 49, ProductId = 49, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 50, ProductId = 50, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 51, ProductId = 51, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 52, ProductId = 52, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 53, ProductId = 53, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 54, ProductId = 54, ImagePath = "/uploads/sample-home.svg", IsPrimary = true },
            new ProductImage { Id = 55, ProductId = 55, ImagePath = "/uploads/sample-home.svg", IsPrimary = true });
    }
}
