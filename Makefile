restore: ## Restore project dependencies
	dotnet restore src/Releasy
	dotnet restore tests/Releasy.Test

build: restore ## Build the whole project
	dotnet build src/Releasy
	dotnet build tests/Releasy.Test

run: build ## Run the program
	dotnet run -p src/Releasy

tests: build ## Run unit tests
	dotnet run -p tests/Releasy.Test

clean: ## Clean the whole project
	dotnet clean src/Releasy
	dotnet clean tests/Releasy.Test

help:
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

.DEFAULT_GOAL := help
.PHONY: restore build tests help
