# Big Picture Event Storming

You can see below the result of our Event Storming sessions. Even if the discussions are a lot more valuable than the result, it helps us onboard newcomers and support our storytelling, when explaining what we want to achieve.

Here's our story :

You are a Product Owner or Product Manager. You feed the dev team with features to implement and create value to your product. Sometimes, a feature takes quite long. Developers are progressing and identify tasks along the way, could it be refactoring or adding a little bit of value. They commit code and create Merge Requests.

To be able to follow how features are progressing, Merge Requests are linked to features using a pattern in the MR description like "contributes to feature #34", "closes feature #42". There can only have one closing MR for a given feature. When the link is established, we can follow the MR progression in the feature. Once the MR is ready to deploy, two things can happen:

- Either the MR only contributes to the feature, and it can directly starts its integration process to production. The MR becomes a change set, which can be queued to Non Regression Testing.
- Or the MR closes a feature, and the feature has to go through a validation process by the PO/PM, the QA or both. Once validated, the corresponding change set can go through its integration process, starting by the NRT. If the feature is not validated, and needs fix, the corresponding MR can be updated (or another MR created and integrated), and when ready, the feature can re perform the validation process.

Once a change set is ready to be deployed to NRT, the MR is rebased, the change set is queued until the NRT environment is available. Once the NRT is available, the change set is unqueued, deployed to the NRT environment and validated. It can be validated or rejected. Once validated, the change set is appended to the current version, and the corresponding MR is merged. When asked, the current version is deployed to production, and a new version is prepared.

In the meantime, alerts have been raised, and contributors have been warned if needed, whatever the reason. Time taken and events have been tracked, and reported to get insights and statistics for the project.

_Pictures are coming soon..._
