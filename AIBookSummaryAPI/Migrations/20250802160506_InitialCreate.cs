using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIBookSummaryAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChapterAnalysis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChapterIndex = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Themes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterAnalysis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChapterInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnalysisId = table.Column<long>(type: "bigint", nullable: true),
                    BookId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterInfo_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChapterInfo_ChapterAnalysis_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "ChapterAnalysis",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CharacterInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChaptersIntroduced = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChapterAnalysisId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterInfo_ChapterAnalysis_ChapterAnalysisId",
                        column: x => x.ChapterAnalysisId,
                        principalTable: "ChapterAnalysis",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NamedLocation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChaptersIntroduced = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChapterAnalysisId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamedLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NamedLocation_ChapterAnalysis_ChapterAnalysisId",
                        column: x => x.ChapterAnalysisId,
                        principalTable: "ChapterAnalysis",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorldVocabItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BestGuessAtMeaning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChapterAnalysisId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldVocabItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorldVocabItem_ChapterAnalysis_ChapterAnalysisId",
                        column: x => x.ChapterAnalysisId,
                        principalTable: "ChapterAnalysis",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CharacterProgression",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Chapter = table.Column<int>(type: "int", nullable: false),
                    Change = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterInfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterProgression", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterProgression_CharacterInfo_CharacterInfoId",
                        column: x => x.CharacterInfoId,
                        principalTable: "CharacterInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChapterInfo_AnalysisId",
                table: "ChapterInfo",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterInfo_BookId",
                table: "ChapterInfo",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterInfo_ChapterAnalysisId",
                table: "CharacterInfo",
                column: "ChapterAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProgression_CharacterInfoId",
                table: "CharacterProgression",
                column: "CharacterInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_NamedLocation_ChapterAnalysisId",
                table: "NamedLocation",
                column: "ChapterAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_WorldVocabItem_ChapterAnalysisId",
                table: "WorldVocabItem",
                column: "ChapterAnalysisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterInfo");

            migrationBuilder.DropTable(
                name: "CharacterProgression");

            migrationBuilder.DropTable(
                name: "NamedLocation");

            migrationBuilder.DropTable(
                name: "WorldVocabItem");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "CharacterInfo");

            migrationBuilder.DropTable(
                name: "ChapterAnalysis");
        }
    }
}
