# Data Privacy & Public Release Policy

This document records exactly what was found, what was changed, and why. Nothing was removed or altered without a stated reason; nothing sensitive found is reproduced here in full (values are described, not quoted verbatim where reproducing them would itself be a leak).

## Removed — real credentials / secrets

| Item | Location | Finding | Action |
|---|---|---|---|
| SQL Server credentials + machine name | `Web.config`, `connectionStrings` | Connection string contained a real `sa` account password and a real internal machine name (`CTH-LTANH\SQLEXPRESS`) | Replaced with placeholders (`YOUR_SERVER_NAME`, `YOUR_DB_USER`, `YOUR_DB_PASSWORD`); original preserved as `Web.config.example` |
| SMTP mail credential | `Common/appsettings.json` | A `MailSettings` block containing a real government email address, the real developer's display name, a plaintext password, and an SMTP host/port. **Verified this file is never read by any code path** (no reference to `appsettings.json` or `MailSettings` anywhere in the C# source) — it is orphaned configuration, not a live credential in use by the running application, but its content is real-looking and sensitive regardless | File deleted entirely; corresponding `<Content Include>` removed from `.csproj` |
| CKFinder commercial license key + licensee name | `ckfinder/config.ascx` | `LicenseName`/`LicenseKey` fields contained a real-looking commercial license key and a real person's name | Both fields cleared (CKFinder falls back to Demo Mode, which is the documented, intended behavior when left blank — no functional code path was altered) |
| Real email address in dead comments | `Common/Function.cs` (5 occurrences), `Common/MailHelper.cs` (3 occurrences) | The same government email address appeared inside commented-out example code | Replaced with a generic placeholder email in all occurrences |

## Removed — real uploaded content

| Item | Location | Finding | Action |
|---|---|---|---|
| 132 real uploaded files | `Upload/images/`, `Upload/files/`, `Upload/_thumbs/`, `Upload/flash/` | This is CKFinder's real upload target directory from actual use of the application, not seed/demo data. Two files inspected visually (chosen because their filenames suggested user-avatar images) turned out to be screenshots of what appears to be **internal tax-administration data** (an e-invoice registration completion table broken down by district tax office, and an internal document-approval workflow diagram) — i.e., real government-related content, unrelated to the LMS itself, apparently uploaded ad hoc while testing the image-upload feature. A separate file contained an unverified phone number in what looks like a stock placeholder graphic. Text files under `Upload/files/` (`Question-*.txt`) contain real, substantive course quiz content (engineering/mechanics topics), not placeholder text. None of this content's provenance or right to publish could be verified. | Entire contents of `Upload/` removed (132 files). Folder structure preserved via `.gitkeep` so the application's expected upload paths remain documented; `.gitignore` updated to prevent future uploaded content from being committed except the placeholder files. |

## Removed — IDE / build artifacts

`.vs/`, `bin/`, `obj/`, `packages/` (240 MB, including multiple superseded package versions and `.deleteme` leftover files from NuGet updates), `LmpSystem.csproj.user` (contained an absolute developer path), and the entire `Properties/PublishProfiles/` folder (10 publish profiles × 2 files each, all containing absolute `D:\` or `F:\` developer paths).

## Kept, with a disclosed note

| Item | Location | Note |
|---|---|---|
| Auto-generated T4 comment path | `Models/ModelLmpSystem.Designer.cs`, line 1: `// T4 code generation is enabled for model 'F:\WorkSpace\LmpSystem\...'` | This is Entity Framework Database-First designer-generated code. It was not hand-edited, consistent with this release's policy of not modifying auto-generated files (regenerating the model in Visual Studio would restore the same kind of path). It contains no credentials, only a folder path. Disclosed here rather than silently left unexplained. |

## Not independently verified

- Whether any of the 132 removed upload files, beyond the ones directly inspected, contained additional sensitive content — a full manual review of every file was not performed; removal was based on (a) the upload folder being demonstrably real usage data rather than curated samples, and (b) a sufficient, confirmed sample of genuinely sensitive content to justify not publishing any of it.
- Whether the CKFinder license key found was still active/valid at the time of this review — it was removed regardless, since a real-looking commercial license key should not be published either way.
- The exact scope of what the SQL Server `sa` account had access to on the original network.
