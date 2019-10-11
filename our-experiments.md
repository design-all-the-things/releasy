# Our Experiments

As we mentioned [in the presentation document](join-us.md), **_Design all the things_** is an initiative to experiment a _Domain Driven Design_ approach to build software. We try to use and learn principles, practices and workshops which could help us better understand the problem and the corresponding domain, which _**Releasy**_ deals with.

Here is a summary of what we tried to explore the domain.

## Event Storming

As a forming group, it's always hard to gather the right people in order to make an _event Storming_ session efficient. Fortunately, the problem we try to solve here is one of those we, as developers, usually face a lot. So we can both act as tech experts and domain experts (I hope we can).

So we started with a _Big Picture_ Event Storming, to get a good overview of how our system would work. We needed at least four or five 1-hour sessions to get something on which we could agree on. We started with events, but quickly we had to make choices on what problem we wanted to solve. Our system deals with a few workflows which can be represented using state diagrams. We use them to get better insights to be more exhaustive with our events.

We didn't try to solve all the problems and questions we faced, and the work could have been done deeper on some aspects, but that was not the point. We got enough insights to move forward, and try something new.

Here is a couple of feedbacks, or maybe lessons:

- Dealing with a technical domain, even one where developers can make good domain experts is tough: everyone has a different view on what is important, and what should be done. Fortunately, Sylvain, our Product Owner, is here to orient on the right path to the problem we need to solve.
- We may have started an Event Storming session too early in the process. We lack a clear understanding of what problem we wanted to solve, how our solution could solve that problem, and for whom. That may explain how _long_ our Event Storming was. But interestingly, this has also make it very explicit, so we didn't hesitate to make a pause on our session and try another workshop, to get a different view on our problem.
- The fact that our session was sliced, and with attendees turn over can also explain why it takes so much time. But at least, we could use it to tell our story to the new ones, and it helped us better understanding it, time after time.

You can find [the result here](design/event-storming/big-picture.md).

## Story Mapping

At some point, we thought that we missed a view of what was important, and what could make a good impact throughout our journey to the solution. We wondered what was core in our domain.

So we tried to start a Story Mapping session. We identified a couple of epics, some activities, and finally, for a few of them, some user stories. The goal was to achieve a minimal story to start with, which eventually crossed many activities. We identified a couple of stories. But because the dedicated session was just an hour long, we were not exhaustive at all.

Here's some feedbacks about this workshop:

- Start with **WHY**, and probably _WHO_. Best advice ever. And it will help you be more accurate and probably more efficient
- If we were not so exhaustive, at least, it leaded us to a minimal story to tell in our solution, which is exactly what we expected from the workshop in the first place. We have recorded everything in a trello board, and we will be able to continue it later.

Our Story Mapping session has been captured in [this opened Trello board](https://trello.com/b/ZotT81x6/story-map).

## Context Mapping

Once we had enough insights with the Big Picture Event Storming, we tried to figure out what was the main Bounded Contexts in our domain. So we identified a couple of key events on which the whole workflow was articulated. This helped us identify a few of them.

We obviously try to understand the words used to describe each concept, and the definition of these words. This also helped make boundaries more concrete.

But even with this, there was still on aspect of the problem which didn't fit well in any identified Bounded Context. So we made the assumption that there was a missing context, and try to imagine its responsibilities, and which concepts were part of it. We finally realized that with it, we could solve unsolved problems.

After identifying Bounded Contexts, we put them on a map, trying to determine how they are related to each other, in a _Context Mapping_ workshop. We stopped at some point, with some questions on how it should evolve, and what pattern better describes each relation.

Some feedbacks we got:

- Context Mapping is an awesome tool. If it helps you describe and better understand an existing situation, we faced some difficulties to define which context was upstream and which one was downstream, for a few connections. We also had some problem defining which patterns were better describing the relation between two contexts. But that may emerge later.
- As other workshops, Context Mapping can (has to?) be done iteratively. Getting insights from other perspectives helps being more relevant when practicing it.
- We tried to follow [Nick Tune's context canvas](https://medium.com/nick-tune-tech-strategy-blog/modelling-bounded-contexts-with-the-bounded-context-design-canvas-a-workshop-recipe-1f123e592ab), but we didn't fully followed it for now. At least, it inspired us to start describing our bounded contexts.

You can access the result [here, in the dedicated section](design/contexts-ul/context-map.jpg).

## Example Mapping

As we moved forward, and after 6 or 7 meetings, we wanted to go deeper in the product, and start delving into a first feature, the one we identified as the most important for now.

To get better insights, we had an _Example Mapping_ session on it. In the beginning, we thought this feature was an obvious one. But after a few discussions and some "What if...?", we finally challenged the story scope. We ended up with some compromises to avoid having an endless story.

A few feedbacks:

- We should capture every idea by writing an example, even if it's thrown away later because it's irrelevant. At least, use a red card to write down a question. This helps being more exhaustive, and avoid forgetting things later on, once the workshop is over.

