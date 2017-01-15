using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using ToDoTnet.DataEntities;

namespace ToDoTnet.Migrations
{
    [DbContext(typeof(ToDoContext))]
    partial class ToDoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("ToDoTnet.DataEntities.ToDo", b =>
                {
                    b.HasOne("ToDoTnet.DataEntities.User")
                        .WithMany("ToDos")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
