﻿// <auto-generated />
using System;
using Fayble.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Fayble.Infrastructure.Migrations
{
    [DbContext(typeof(FaybleDbContext))]
    partial class FaybleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("Fayble.Domain.Aggregates.BackgroundTask.BackgroundTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Started")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("StartedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BackgroundTask", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Book.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly?>("CoverDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileFormat")
                        .HasColumnType("TEXT");

                    b.Property<string>("FilePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("Filename")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("FormatId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Language")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastMetadataUpdate")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("LibraryId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("LibraryPathId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Locked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MediaPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("MediaType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PageCount")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("PublisherId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Rating")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Review")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("SeriesId")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly?>("StoreDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FormatId");

                    b.HasIndex("LibraryId");

                    b.HasIndex("LibraryPathId");

                    b.HasIndex("PublisherId");

                    b.HasIndex("SeriesId");

                    b.ToTable("Book", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Book.ReadHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BookId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ReadDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("ReadHistory");
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Configuration.Configuration", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Configuration", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.FileType.FileType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileExtension")
                        .HasColumnType("TEXT");

                    b.Property<string>("LibraryType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FileType", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Format.Format", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("MediaType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Format", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Library.Library", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Library", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Library.LibraryPath", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LibraryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LibraryId");

                    b.ToTable("LibraryPath", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Library.LibrarySetting", b =>
                {
                    b.Property<string>("Setting")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LibraryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Setting", "LibraryId");

                    b.HasIndex("LibraryId");

                    b.ToTable("LibrarySetting", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Publisher.Publisher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("LastMetadataUpdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("MediaPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Publisher", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.RefreshToken.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Expiration")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Revoked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Series.Series", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("FormatId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastMetadataUpdate")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("LibraryId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Locked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MediaPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ParentSeriesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PublisherId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Rating")
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Volume")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FormatId");

                    b.HasIndex("LibraryId");

                    b.HasIndex("ParentSeriesId");

                    b.HasIndex("PublisherId");

                    b.ToTable("Series", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.User.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoleClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken", (string)null);
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Book.Book", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.Format.Format", "Format")
                        .WithMany()
                        .HasForeignKey("FormatId");

                    b.HasOne("Fayble.Domain.Aggregates.Library.Library", "Library")
                        .WithMany("Books")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fayble.Domain.Aggregates.Library.LibraryPath", "LibraryPath")
                        .WithMany()
                        .HasForeignKey("LibraryPathId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fayble.Domain.Aggregates.Publisher.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId");

                    b.HasOne("Fayble.Domain.Aggregates.Series.Series", "Series")
                        .WithMany("Books")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Format");

                    b.Navigation("Library");

                    b.Navigation("LibraryPath");

                    b.Navigation("Publisher");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Book.ReadHistory", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.Book.Book", null)
                        .WithMany("ReadHistory")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fayble.Domain.Aggregates.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Library.LibraryPath", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.Library.Library", "Library")
                        .WithMany("Paths")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Library.LibrarySetting", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.Library.Library", "Library")
                        .WithMany("Settings")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Library");
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.RefreshToken.RefreshToken", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Series.Series", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.Format.Format", "Format")
                        .WithMany()
                        .HasForeignKey("FormatId");

                    b.HasOne("Fayble.Domain.Aggregates.Library.Library", "Library")
                        .WithMany("Series")
                        .HasForeignKey("LibraryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fayble.Domain.Aggregates.Series.Series", "ParentSeries")
                        .WithMany()
                        .HasForeignKey("ParentSeriesId");

                    b.HasOne("Fayble.Domain.Aggregates.Publisher.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId");

                    b.Navigation("Format");

                    b.Navigation("Library");

                    b.Navigation("ParentSeries");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.User.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.User.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.User.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.User.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fayble.Domain.Aggregates.User.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Fayble.Domain.Aggregates.User.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Book.Book", b =>
                {
                    b.Navigation("ReadHistory");
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Library.Library", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("Paths");

                    b.Navigation("Series");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("Fayble.Domain.Aggregates.Series.Series", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
