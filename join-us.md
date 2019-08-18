# Presentation

**_Design all the things_** is an open initiative imagined by [Sylvain Fraïssé](https://github.com/fraisse) and [Jérôme Avoustin](https://github.com/rehia) to explore and learn how a "true" _Domain Driven Design_ approach could help us write better softwares. The purpose is not to build a product, but to learn new things, and make our mental models evolve.

It is organized around face-to-face workshops, and probably remote as well. They are open to anyone who wants to contribute. You can come and contribute to the whole project, or just come once. We will learn together. If you want to know more, join the [_Montpellier Communautés_ slack](https://bit.ly/comm-mtp), and follow the #ddd channel (_Design all the things_ keyword). We organize meetups on it.

## The Product

Because we can't follow a _Domain Driven Design_ approach if we don't have at least a problem to solve, we came with one which presents at least 3 main interests:

1. It's a problem we, as developers, meet every day, and we have gathered some knowledge about, making us legitimate product owners
2. We will use the solution to build it, and _eat our own dog food_
3. The problem is complex enough, and the domain language rich enough to justify such an approach.

_**Releasy**_ is a system intended to help product owners/managers, developers and other team members give a better transparency on _how _[the] _project is coming along_, and try to better understand how their build workflow is going on, from the feature declaration to production.

![Dilbert](https://assets.amuniversal.com/ad9252606cc801301d50001dd8b71c47)

Releasy does not pretend to replace any task management system. It will be the _glue_, or more probably the _oil_ between a task management system (Trello, Jira...), a VCS (Gthub, Gitlab...), a CI (Travis, Circle CI, Jenkins, Gitlab CI...), a team communication system (Slack, Mattermost..), metrics/analytics tools (Kibana, Grafana, Prometheus...), and your environments (staging, non regression, production).

Releasy will eventually help automate some tasks, for example moving cards in trello, trigger automated jobs, like deployments, send messages to team members, reports some metrics...
