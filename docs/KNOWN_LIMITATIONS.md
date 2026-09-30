# Known Limitations

Stated deliberately, based on direct code review — not hidden or downplayed, and not fixed as part of this release (per its scope: public-safety and documentation cleanup, not a rewrite).

## Security

- **Quiz scoring trusts the client**: `TestQuiz`/`SaveTestResult` (`Controllers/HomeController.cs`) accept the candidate's score directly from the request and persist it without recomputing it from the actual questions/answers. A learner could submit an arbitrary score.
- **Unsalted MD5 password hashing** (`Common/Function.cs`, `CalculateMD5Hash`): adequate to demonstrate the authentication flow, not adequate for a production system with real accounts.
- **Custom role provider is incomplete**: `MyRoleProvider` is registered in `Web.config` but most of its methods throw `NotImplementedException`. In practice, role gating works via `[Authorize(Roles = "admin")]` against the Forms Authentication ticket, not through a fully functional custom provider — this is fragile if the provider is ever invoked in a code path that actually calls its unimplemented methods.

## Data model

- **Unnormalized quiz answers**: `UserScore.TestDetail` is a delimited string, not a normalized per-question-per-attempt table, which blocks aggregate analysis without a schema change.
- No database migrations history exists — this is a Database-First EF model with no standalone `.sql` schema script included in this release.

## Functionality

- No automated tests exist anywhere in the codebase.
- No API layer; this is a single server-rendered MVC application.
- The `Upload/` directory's real contents were removed for this public release (see `docs/DATA_PRIVACY.md`), so running this application locally will start with an empty upload directory — this is expected, not a bug.

## Not verified in this release

- The project was not built or run (no Windows/MSBuild environment was available during preparation).
- CKFinder's behavior in Demo Mode (with the license key removed) was not tested end-to-end; per CKFinder's own documented behavior, Demo Mode should remain "fully functional" for local development.
