Tables:
- Recipe

	| Field Type | Field Name | Keys
	| --- | --- | --- |
	| Guid | Id | Primary Key |
	| String | Title | |
	| Guid | UserId | Foreign Key => (User.Id) |
- User

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | Id | Primary Key |
	| String | Name |  |
	| String | PasswordHash | |
	| Guid | RefreshTokenId | Foreign Key => (RefreshToken.Id) |
- Category

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | Id | Primary Key |
	| String | Data |  |
- RecipeCategory

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | Id | Primary Key |
	| String | Data |  |
	| Guid | RecipeId | Foreign Key => (Recipe.Id) |
- Instruction

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | Id | Primary Key |
	| String | Data |  |
	| Guid | RecipeId | Foreign Key => (Recipe.Id) |
- Ingredient

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | Id | Primary Key |
	| String | Data |  |
	| Guid | RecipeId | Foreign Key => (Recipe.Id) |
- RefreshToken

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | Id | Primary Key |
	| String | Token |  |
	| TimeStamp | TimeCreated |  |
	| TimeStamp | TimeExpires |  |

Relationships:

| Type | Tables involved 
| --- | --- |
| One to Many | Recipe to instruction |
| One to Many | Recipe to Ingredient |
| One to Many | Recipe to RecipeCategory |
| One to Many | User to Recipe |
| One to One | User to RefreshToken |