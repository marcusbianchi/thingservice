﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using thingservice.Data;

namespace thingservice.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171107185537_EnableRemoveParameters")]
    partial class EnableRemoveParameters
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("thingservice.Model.Parameter", b =>
                {
                    b.Property<int>("parameterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ParameterCode")
                        .HasMaxLength(50);

                    b.Property<string>("parameterDescription")
                        .HasMaxLength(100);

                    b.Property<string>("parameterName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("physicalTag")
                        .HasMaxLength(100);

                    b.Property<int>("thingGroupId");

                    b.HasKey("parameterId");

                    b.HasIndex("thingGroupId");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("thingservice.Model.Thing", b =>
                {
                    b.Property<int>("thingId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(100);

                    b.Property<int[]>("childrenThingsIds")
                        .HasColumnName("childrenThingsIds")
                        .HasColumnType("integer[]");

                    b.Property<bool>("enabled");

                    b.Property<int?>("parentThingId");

                    b.Property<string>("physicalConnection")
                        .HasMaxLength(100);

                    b.Property<int>("position")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("thingCode")
                        .HasMaxLength(50);

                    b.Property<string>("thingName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("thingId");

                    b.ToTable("Things");
                });

            modelBuilder.Entity("thingservice.Model.ThingGroup", b =>
                {
                    b.Property<int>("thingGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("enabled");

                    b.Property<string>("groupCode")
                        .HasMaxLength(50);

                    b.Property<string>("groupDescription")
                        .HasMaxLength(100);

                    b.Property<string>("groupName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int[]>("thingsIds")
                        .HasColumnName("thingsIds")
                        .HasColumnType("integer[]");

                    b.HasKey("thingGroupId");

                    b.ToTable("ThingGroups");
                });

            modelBuilder.Entity("thingservice.Model.Parameter", b =>
                {
                    b.HasOne("thingservice.Model.ThingGroup")
                        .WithMany("paremeters")
                        .HasForeignKey("thingGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}