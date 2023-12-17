# POC Overview

## Motivation
* Demonstrate Contract testing in an isolated setup.
* Favor high level unit tests and shifting left since all examples available are very basic.
* Provide helpers that let this solution scale:
  * Adding new pacts and downstream services in an isolated way.
  * Clearly defines valid and invalid behaviors to be used in tests. Effective when some tests requires a different setup (simulating errors etc...) 
  * Leverages the same startup and wiring used in the production application while reducing mocking and manual implementations outside of pacts.
  * A central place to define and register pacts.
  * Accomodate complex pacts and services, while keeping the tests simple and promoting re-usability.
* For low level unit tests, see
  * [consumer tests](https://github.com/pact-foundation/pact-net/blob/master/samples/OrdersApi/Consumer.Tests/OrdersClientTests.cs)
  * [provider tests](https://github.com/pact-foundation/pact-net/blob/master/samples/OrdersApi/Provider.Tests/ProviderTests.cs)

## Implementation
* Everything in Internal would be moved to a dedicated nuget.
* Tiny optimizations remains to be done and are labeled in TODOs in the code.
* PACT broker would be setup to send and validate contracts between the provider and consumers. This only focuses on the actual contracts being validated.
