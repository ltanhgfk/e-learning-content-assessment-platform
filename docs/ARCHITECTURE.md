# Architecture

## Verified structure

```
Browser
   ↓
ASP.NET MVC Controllers
   Controllers/HomeController.cs           (public site: courses, lessons, quiz, cart)
   Areas/Admin/Controllers/*.cs            (admin: content & user management, role-gated)
   ↓
Repository + Unit of Work (Repository/UnitOfWork.cs)
   - Lazily exposes per-entity repositories (UserRepository, LessonRepository,
     QuestionRepository, OrderRepository, PostRepository, ...) sharing one
     LmpSystemEntities (EF6) context
   - Explicit Commit() boundary
   ↓
Entity Framework 6 (Database-First)
   ↓
SQL Server
```

ViewModels (`ViewModels/*.cs`) are used by controllers instead of exposing EF entity classes directly to Views — a deliberate separation-of-concerns choice, verified across multiple controllers (e.g., `LessonEditViewModel`, `QuestionViewModel`, `UserEditViewModel`).

## Authentication & Authorization

Standard ASP.NET Forms Authentication (`Web.config`: `<authentication mode="Forms">`, `loginUrl="Admin/Home/Login"`) with role-based `[Authorize(Roles = "admin")]` attributes on admin controllers (`Areas/Admin/Controllers/*.cs`). A custom `MyRoleProvider : RoleProvider` is registered but most of its methods throw `NotImplementedException` — role checks in practice rely on the `[Authorize(Roles=...)]` attribute working against the Forms Authentication ticket, not on a fully implemented custom role provider.

## What this architecture is — and is not

- **Is**: a two-tier MVC application with a genuine data-access abstraction layer (Repository + Unit of Work) between controllers and Entity Framework.
- **Is not**: distributed, microservice, or API-based. There is no service layer beyond the repositories, no message queue, no separate backend service.
- **Is not** AI/ML: no statistical or learned model exists anywhere in the codebase.

## Third-party components

- **CKEditor / CKFinder**: vendored (not NuGet-restored) rich-text editing and file-management libraries under `ckeditor/` and `ckfinder/`. CKFinder requires a license key to leave "Demo Mode" in production; the real key found in the original source was removed (see `docs/DATA_PRIVACY.md`).
- **PagedList / PagedList.Mvc**: server-side pagination.
