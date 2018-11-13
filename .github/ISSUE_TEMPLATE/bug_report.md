---
name: Bug report
about: Create a report to help us improve
labels: 

---

**Describe the bug**
A clear and concise description of what the bug is.

**To Reproduce**
Add a complete standalone code that reproduces the issue. Preferably upload it as a Gist to [GitHub Gist](https://gist.github.com/). Examples [#1](https://gist.github.com/darkl/8546e3abfce64f43b635574e9708e7f0), [#2](https://gist.github.com/darkl/7db5cb48e235bf5b08fa17723aa61677), [#3](https://gist.github.com/esso23/f04702909ac40799075b5221b406b009), [#4](https://gist.github.com/BramVader/864655dc8edb087bb015e143cc6589cb).

Additionally, what steps should be done in order to reproduce the issue.

For example:
1. Run crossbario router.
2. Run Callee.cs
3. Run Caller.cs
4. Kill crossbario router.
5. Run crossbario router again.
6. Run Caller.cs again.

Vague bug reports with poorly formatted code snippets and/or non-standalone code snippets, will be automatically closed!

**Expected behavior**
A clear and concise description of what you expected to happen.

For example:
Expected: Caller.cs CalleeProxy call should succeed, but it fails.

**External WAMP libraries involved**
Specify any other WAMP technology involved and explain how.

Examples:
* crossbar.io 18.11.1 is used as a router with the following config.json (add config file to your gist).
* AutobahnJS 18.10.2 is used as a callee (add callee code to your gist).

**.NET platform variant**
Specify your .NET platform variant. This includes your operating system version.

Examples:
* .NET Framework 4.6.3 on Windows 10
* .NET Core 2.1 on Ubuntu 18.10
* Xamarin.Android 9
* UWP Build 17763 (version 1809)

**Additional context**
Add any other context about the problem here.
