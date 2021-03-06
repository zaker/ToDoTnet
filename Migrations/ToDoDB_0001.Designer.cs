﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ToDoTnet.DataEntities;

namespace ToDoTnet.Migrations
{
    [DbContext(typeof(ToDoContext))]
    [Migration("ToDoDB_0001")]
    partial class ToDoDB_0001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("ToDoTnet.DataEntities.ToDo", b =>
                {
                    b.Property<Guid>("ToDoID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime?>("DoneDate");

                    b.Property<int>("Priority");

                    b.Property<string>("Product");

                    b.Property<string>("Title");

                    b.Property<string>("Type");

                    b.Property<Guid>("UserID");

                    b.HasKey("ToDoID");

                    b.HasIndex("UserID");

                    b.ToTable("ToDos");
                });

            modelBuilder.Entity("ToDoTnet.DataEntities.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });


        }
    }
}
