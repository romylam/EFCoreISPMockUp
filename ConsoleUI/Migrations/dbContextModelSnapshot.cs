﻿// <auto-generated />
using System;
using ConsoleUI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleUI.Migrations
{
    [DbContext(typeof(dbContext))]
    partial class dbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0-preview.6.21352.1");

            modelBuilder.Entity("ConsoleUI.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Credit")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Debit")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Open")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Transfer")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Account");
                });

            modelBuilder.Entity("ConsoleUI.MasterKey", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("NextKey")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NextTerm")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Prefix")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactor")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("MasterKeys");
                });

            modelBuilder.Entity("ConsoleUI.CreditAccount", b =>
                {
                    b.HasBaseType("ConsoleUI.Account");

                    b.Property<decimal>("Limit")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("CreditAccount");
                });

            modelBuilder.Entity("ConsoleUI.GeneralAccount", b =>
                {
                    b.HasBaseType("ConsoleUI.Account");

                    b.HasDiscriminator().HasValue("GeneralAccount");
                });

            modelBuilder.Entity("ConsoleUI.TradingAccount", b =>
                {
                    b.HasBaseType("ConsoleUI.Account");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("PriceDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("TradingAccount");
                });
#pragma warning restore 612, 618
        }
    }
}
