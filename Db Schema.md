Tables:
- recipe

	| Field Type | Field Name | Keys
	| --- | --- | --- |
	| Guid | id | Primary Key |
	| String | title | |
	| Guid | user_id | Foreign Key => (user.id) |
	| Boolean | is_active | |
- user

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | id | Primary Key |
	| String | name |  |
	| String | password_hash | |
	| Guid | refresh_token_id | Foreign Key => (refresh_token.id) |
	| Boolean | is_active | |
- category

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | id | Primary Key |
	| String | data |  |
	| Boolean | is_active | |
- recipe_category

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | id | Primary Key |
	| String | data |  |
	| Guid | recipe_id | Foreign Key => (recipe.id) |
	| Boolean | is_active | |
- instruction

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | id | Primary Key |
	| String | data |  |
	| Guid | recipe_id | Foreign Key => (recipe.id) |
	| Boolean | is_active | |
- ingredient

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid | id | Primary Key |
	| String | data |  |
	| Guid | recipe_id | Foreign Key => (recipe.id) |
	| Boolean | is_active | |
- refresh_token

	| Field Type | Field Name | Keys
	| --- | --- | --- |
    | Guid |id | Primary Key |
	| String | token |  |
	| TimeStamp | time_created |  |
	| TimeStamp | time_expires |  |
	| Boolean | is_active | |
Relationships:

| Type | Tables involved 
| --- | --- |
| One to Many | recipe to instruction |
| One to Many | recipe to Ingredient |
| One to Many | recipe to recipe_category |
| One to Many | user to recipe |
| One to One | user to refresh_token |