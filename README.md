# E-Learning Content & Assessment Platform

A legacy ASP.NET MVC learning platform combining course content management, quiz-based assessment, user roles, and course purchasing workflows.

This repository is published as a research/portfolio artifact documenting real system-design work. It is not maintained as a commercial product, is not being modernized as part of this release, and makes **no claim of AI/ML capability** — none exists in the source.

## Overview

The system lets instructors publish courses composed of lessons (video, text content, exercises), attach multiple-choice questions to lessons, and lets learners register, purchase courses, and take quizzes, with results stored for later review. An admin area, separated via ASP.NET MVC Areas, manages all content and users.

## Content Management

Content is organized as a hierarchy: **Course (`Post`) → Lesson → Question**, with `Category` and `Tag` for classification. Lesson content is authored with CKEditor, and images/files are uploaded via CKFinder into an `Upload/` directory (see [Data Privacy](#data-privacy--public-release-policy) for what was removed from that folder before this release).

## Assessment Engine

Each lesson can have multiple-choice questions (`Question`: stem + 4 options + one correct answer). A learner's quiz attempt is recorded in `UserScore` (total/correct answer counts, plus a delimited per-question answer string). This is a straightforward multiple-choice quiz engine — **verified in source** to have no adaptive logic, no item-difficulty modeling, and no machine learning of any kind.

## Repository / Unit of Work

The data-access layer is a genuine **Repository + Unit of Work** implementation, verified directly in source: `Repository/UnitOfWork.cs` lazily exposes 13+ per-entity repositories (`UserRepository`, `LessonRepository`, `QuestionRepository`, `OrderRepository`, etc.) sharing one `LmpSystemEntities` (Entity Framework 6) context, with an explicit `Commit()` boundary. Controllers depend on `UnitOfWork`, not on the EF context directly. Dedicated `ViewModel` classes (e.g., `LessonEditViewModel`, `QuestionViewModel`) are used instead of exposing EF entities to views.

## Role-Based Administration

The public site and the admin area are split via ASP.NET MVC **Areas** (`Areas/Admin`). Admin controllers use standard `[Authorize(Roles = "admin")]` attributes with ASP.NET Forms Authentication — **verified in source**, not a custom session hack.

## Data Model (core entities)

| Entity | Role |
|---|---|
| `User` | Learner/instructor/admin, with a role field |
| `Post` | A course |
| `Lesson` | A unit of content within a course (video, text, exercises) |
| `Question` | A multiple-choice question tied to a lesson (4 options, 1 correct) |
| `UserScore` | A quiz attempt: total/correct answer counts + a delimited per-question answer string |
| `Cart` / `Order` / `Invoice` | Course purchase flow |
| `Category` / `Tag` | Content classification |

Full detail in [`docs/DATA_MODEL.md`](docs/DATA_MODEL.md).

## Known Limitations

Stated deliberately, based on direct code review:

- **Quiz scoring integrity**: quiz results (`TrueNumberAnswer`, `TotalNumberQuestion`) are computed client-side and submitted to the server via `TestQuiz`/`SaveTestResult`, which persist the submitted values **without recomputing them from the actual question/answer data**. Verified directly in `Controllers/HomeController.cs` — this is a real data-integrity gap, not a hypothetical one.
- **Password hashing**: administrator/user passwords are hashed with **unsalted MD5** (`Common/Function.cs`, `CalculateMD5Hash`) — verified in source. Adequate to demonstrate the authentication flow, not adequate for production use.
- **Unnormalized quiz data**: `UserScore.TestDetail` stores per-question answers as a delimited string (`questionId:answer#...`) rather than in a normalized table, which blocks aggregate analysis (e.g., per-question difficulty) without a schema migration.
- No automated tests exist.

None of these were fixed as part of this release, per its scope (public-safety and documentation cleanup, not a rewrite) — see [`docs/KNOWN_LIMITATIONS.md`](docs/KNOWN_LIMITATIONS.md).

## Historical / Portfolio Nature

This is a legacy project, not built for public release from the outset. It is published here as-is, with sensitive data and credentials removed, to document real content-management and system-design work. No modernization (framework upgrade, added AI, added CI/CD, rewritten architecture, rewritten authentication) has been performed as part of this release.

## Repository Structure

```
.
├── README.md
├── .gitignore
├── docs/
│   ├── ARCHITECTURE.md
│   ├── DATA_MODEL.md
│   ├── DATA_PRIVACY.md
│   ├── KNOWN_LIMITATIONS.md
│   └── ROADMAP.md
└── LmpSystem/
    ├── Areas/Admin/          (admin controllers/views, role-gated)
    ├── Controllers/          (public site — HomeController)
    ├── Models/               (EF entities, Database-First)
    ├── Repository/           (Repository + UnitOfWork pattern)
    ├── ViewModels/
    ├── Views/
    ├── Common/               (helper classes — MD5 hashing, mail helper stub)
    ├── ckeditor/, ckfinder/  (vendored rich-text/file-manager libraries)
    ├── Upload/               (images/, files/ kept as empty structure — see Data Privacy)
    ├── Web.config.example
    └── LmpSystem.csproj
```

## Technology Stack

C# · ASP.NET MVC 5 (Areas) · .NET Framework 4.7.2 · Entity Framework 6 (Database-First) · SQL Server · Repository/Unit-of-Work pattern · CKEditor/CKFinder · PagedList.

## Configuration

`Web.config` previously contained a live database connection string with real credentials and a real internal machine name; these have been replaced with placeholders. Copy `LmpSystem/Web.config.example` to `Web.config` and supply your own SQL Server connection details before running locally. An unrelated, unused SMTP-credential config file (`Common/appsettings.json`) that was never actually read by any code path was removed entirely — see [Data Privacy](#data-privacy--public-release-policy).

## Build Notes

This project targets **.NET Framework 4.7.2** and restores most dependencies via NuGet (`packages.config`). **Build was not verified as part of this release** — the environment used to prepare this repository has no Windows/MSBuild/.NET Framework tooling available. Also note: CKFinder (`ckfinder/config.ascx`) requires a commercial license key to leave Demo Mode; the real license key found in the original source was removed (see Data Privacy) and the field left blank.

## Public Release / Data Policy

Real database credentials, a real SMTP mail credential (in an unused config file), a real commercial license key, a real personal name tied to that license, and 132 real uploaded files (course images, lesson images, and real course quiz content, including material that appears to be internal government documents accidentally uploaded during feature testing) were removed. Local Visual Studio publish-profile files and IDE-local project settings containing absolute developer machine paths were also removed. Full itemized list and rationale in [`docs/DATA_PRIVACY.md`](docs/DATA_PRIVACY.md).
