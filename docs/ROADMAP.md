# Roadmap / Future Directions

Everything below is **future work — nothing described here is implemented in the current codebase.** AI/RAG items in particular are explicitly future work, not existing capabilities, and this repository makes no claim otherwise.

## Phase 1 — Correctness & security hygiene
- Secure password hashing (replace unsalted MD5 with a salted, slow hash such as PBKDF2/bcrypt/Argon2).
- Server-side quiz scoring: recompute `TrueNumberAnswer`/`TotalNumberQuestion` from the stored `Question.Answer` values instead of trusting the client-submitted score.
- Normalize `UserScore.TestDetail` into a proper per-question-per-attempt table.

## Phase 2 — Learning analytics
- Item-level statistics (difficulty, discrimination) computed from aggregated historical quiz attempts, enabled by the Phase 1 normalization.
- Learner progress tracking across lessons/courses over time.

## Phase 3 — Personalization
- Personalized learning recommendations (e.g., suggesting a lesson to revisit) based on quiz performance per topic.

## Phase 4 — Optional, exploratory
- Knowledge tracing (modeling a learner's estimated mastery of a skill over repeated attempts).
- A RAG-based learning assistant scoped to a course, using existing `Lesson.Content` as a retrieval source.

## Explicitly out of scope for this release

Per this release's audit instructions, none of the above was implemented, no AI/ML/RAG component was added anywhere in this codebase, and no large-scale architecture or authentication rewrite was performed as part of preparing this repository for publication.
