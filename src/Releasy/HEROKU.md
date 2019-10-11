# How to deploy to Heroku

For now, the created application on Heroku is `releasy-test`.

## Requirements

You'll need [Heroku Command Line Interface](https://devcenter.heroku.com/articles/heroku-cli#download-and-install).

## Deploy the application

```
$ heroku login
$ heroku container:login
$ heroku container:push web -a releasy-test
$ heroku container:release web -a releasy-test
```

## See what's going on

```
$ heroku login
$ heroku logs --tail -a releasy-test
```
