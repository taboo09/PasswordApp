﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PassApp.API.Persistence;
using System;

namespace PassApp.API.Migrations
{
    [DbContext(typeof(PassDbContext))]
    [Migration("20180612213218_UpdateUserModelKey")]
    partial class UpdateUserModelKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("PassApp.API.Models.Password", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Hint");

                    b.Property<string>("Name");

                    b.Property<string>("Other");

                    b.Property<string>("PasswordEncrypt")
                        .IsRequired();

                    b.Property<string>("SecretQuestion");

                    b.Property<DateTime>("Updated");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Passwords");
                });

            modelBuilder.Entity("PassApp.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Email");

                    b.Property<byte[]>("EntryCodeHash");

                    b.Property<byte[]>("EntryCodeSalt");

                    b.Property<byte[]>("UserKey");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PassApp.API.Models.Password", b =>
                {
                    b.HasOne("PassApp.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
