restore: ## Restore project dependencies
	dotnet restore src/Releasy
	dotnet restore tests/Releasy.Test

build: restore ## Build the whole project
	dotnet build src/Releasy
	dotnet build tests/Releasy.Test

run: build ## Run the program
	dotnet run -p src/Releasy

tests: ## Run unit tests
	dotnet run -p tests/Releasy.Test -f netcoreapp3.0 -c release -- --summary

tests-watch: ## Watch code changes and run unit tests
	dotnet watch -p tests/Releasy.Test run -f netcoreapp3.0 -c release -- --colours 256

clean: ## Clean the whole project
	dotnet clean src/Releasy
	dotnet clean tests/Releasy.Test

help:
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

.DEFAULT_GOAL := help
.PHONY: restore build tests help
