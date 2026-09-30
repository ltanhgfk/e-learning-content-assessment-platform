# Data Model

## Core entities (verified against `Models/*.cs`)

| Entity | Key fields | Role |
|---|---|---|
| `User` | Id, Email, Password (MD5 hash), Role, Status | Learner/instructor/admin account |
| `Post` | Id, Title, Content, CateId | A course |
| `Lesson` | Id, PostId, Title, Content, Video | A unit of content within a course |
| `Question` | Id, LessonId (or PostId), Content, Option1–4, Answer | Multiple-choice question, single correct answer |
| `UserScore` | Id, Email, CateId/LessonId, TrueNumberAnswer, TotalNumberQuestion, TestDetail | One quiz attempt |
| `Cart` | Id, UserId, PostId | Course added to cart |
| `Order` | Id, UserId, ... | A purchase order |
| `Invoice` | Id, OrderId, ... | Invoice for a completed order |
| `Category` / `Tag` | Id, Name | Content classification |

## Quiz data — verified structure and its limitation

`UserScore.TestDetail` stores a delimited string built client-side, in the form:

```
questionId:selectedAnswer#questionId:selectedAnswer#...
```

This is parsed back apart only to redisplay a single past attempt (`Controllers/HomeController.cs`); there is no code path that aggregates this data across users or questions. This is why per-question difficulty/discrimination analysis is listed as future work rather than an existing capability — the data exists per-attempt, but not in a form ready for aggregate analysis.

## Quiz scoring — verified integrity gap

`TestQuiz(UserScore model)` and `SaveTestResult(...)` (`Controllers/HomeController.cs`) both accept `TrueNumberAnswer` and `TotalNumberQuestion` as request parameters and persist them directly, with no server-side recomputation against the actual `Question.Answer` values. This means the score is trusted from the client. This is documented as a known limitation, not fixed as part of this release (see `docs/KNOWN_LIMITATIONS.md`).

## Repository mapping (verified in `Repository/UnitOfWork.cs`)

Each entity above has a corresponding repository (e.g., `UserRepository`, `PostRepository`, `LessonRepository`, `QuestionRepository`, `UserScoreRepository`, `CategoryRepository`, `TagRepository`, `OrderRepository`, `InvoiceRepository`, `PostImageRepository`, `PostReviewRepository`, `PostTagRepository`, `FeedbackRepository`, `LmpInfoRepository`), all sharing one `LmpSystemEntities` context via `UnitOfWork`.
