Feature: Subsidiaries Linkage

Scenario: Creating pending link
	Given Two suppliers SpaceX Europe and SpaceX Poland
	When I create pending link for SpaceX Poland as subsidiary of SpaceX Europe
	Then Pending link is created for SpaceX Poland as subsidiary of SpaceX Europe

Scenario: Creating accepted link
	Given Two suppliers SpaceX Europe and SpaceX Poland
	When I create accepted link for SpaceX Poland as subsidiary of SpaceX Europe
	Then Accepted link is created for SpaceX Poland as subsidiary of SpaceX Europe

Scenario Outline: Accepting link
	Given <status> link between <subsidiary> and <parent>
	When I accept the link
	Then Link is accepted
 Examples:
    | subsidiary	| parent		| status	|
	| SpaceX Poland	| SpaceX Europe	| Pending	|
	| SpaceX Poland | SpaceX Europe | Declined	|

Scenario Outline: Declining link
	Given <status> link between <subsidiary> and <parent>
	When I decline the link
	Then Link is declined
 Examples:
    | subsidiary	| parent		| status	|
	| SpaceX Poland	| SpaceX Europe	| Pending	|
	| SpaceX Poland | SpaceX Europe	| Accepted	|

Scenario Outline: Removing link
	Given <status> link between <subsidiary> and <parent>
	When I remove the link
	Then Link is removed
 Examples:
    | subsidiary	| parent		| status	|
	| SpaceX Poland	| SpaceX Europe	| Pending	|
	| SpaceX Poland	| SpaceX Europe	| Declined	|
	| SpaceX Poland | SpaceX Europe	| Accepted	|

Scenario: Cannot set supplier's subsidiary as a parent
	Given SpaceX Poland as subsidiary of SpaceX Europe
	When I create link for SpaceX Europe as subsidiary of SpaceX Poland
	Then Action is denied

Scenario: Can set supplier's grand subsidiary as parent
	Given SpaceX with subsidiary SpaceX Europe with subsidiary SpaceX Poland
	When I create link for SpaceX as subsidiary of SpaceX Poland
	Then Link is created for SpaceX as subsidiary of SpaceX Poland