Feature: Order billing with time-based discount

  Scenario: Scenario 1A - Order applied in discount period gets discount
    Given an order for a group of 4 people placed at 18:30
    And the order contains 4 starter, 4 mains and 4 drinks
    And a time-based discount applies to items ordered before 19:00
    When the guests request the bill
    Then service charge is applied
    Then the bill total should be 45.90

  Scenario: Scenario 1B - Order applied outside discount period has no discount
    Given an order for a group of 4 people placed at 20:00
    And the order contains 4 starter, 4 mains and 4 drinks
    And no time-based discount applies to items ordered after 19:00
    When the guests request the bill
    Then the bill total should be 48.6

  Scenario: Scenario 2 - Mixed discounted and full-price items in one order
    Given an order for a group of 2 people placed at 18:30
    And the order contains 1 starter, 2 mains and 2 drinks
    And a time-based discount applies to items ordered before 19:00
    When the guests request the bill
    Then the bill total should be 19.35

    When 2 more people join the group at 20:00
    And they order 2 mains and 2 drinks
    Then no discount should be applied to the newly ordered items
    When the guests request the final bill
    Then the bill total should be 36.45

  Scenario: Scenario 3 - Order updated outside discount period keeps applied discount
    Given an order for a group of 4 people placed at 18:15
    And the order contains 4 starter, 4 mains and 4 drinks
    And a time-based discount applies to items ordered before 19:00
    When the guests request the bill
    Then the bill total should be 45.90

    When 1 person cancels their order at 20:00
    And the order is updated to remove 1 starter, 1 main and 1 drink ordered at 18:15
    When the guests request the final bill
    Then the bill total should be 34.42

