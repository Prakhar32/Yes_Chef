Feature: Yes Chef! - Kitchen Cooking Game

  # ─────────────────────────────────────────
  # GAME START
  # ─────────────────────────────────────────

  Scenario: Controls screen shown before game begins
    Given the application has launched
    Then a UI window is displayed showing the available controls
    And a "Start" button is visible
    And the game timer is not running

  Scenario: Game begins when player clicks Start
    Given the controls screen is displayed
    When the player clicks the Start button
    Then the controls screen is dismissed
    And the 3-minute countdown timer begins
    And 4 customer windows each display a new order

  # ─────────────────────────────────────────
  # PLAYER MOVEMENT & INTERACTION
  # ─────────────────────────────────────────

  Scenario: Player moves around the kitchen
    Given the game is running
    When the player provides movement input
    Then the player character moves in the corresponding direction
    And the player is constrained within the kitchen walls

  Scenario: Player opens ingredient selection menu at the refrigerator
    Given the player is holding nothing
    And the player is adjacent to the refrigerator
    When the player interacts with the refrigerator
    Then a world-space ingredient selection menu appears near the refrigerator
    And the menu shows 3 ingredient options and a Back option
    And the player enters selection context (cannot move)

  Scenario: Player selects an ingredient from the refrigerator menu
    Given the ingredient selection menu is open
    When the player navigates to an ingredient option and confirms
    Then the player receives that ingredient in hand
    And the ingredient is visibly held by the player
    And the menu closes
    And the player exits selection context

  Scenario: Player cancels the refrigerator menu
    Given the ingredient selection menu is open
    When the player presses the Cancel button or selects Back
    Then the menu closes
    And the player exits selection context
    And the player's hand remains empty

  Scenario: Player navigates the refrigerator menu using movement keys
    Given the ingredient selection menu is open
    When the player provides movement input
    Then the menu selection highlight moves accordingly
    And the player character does not move

  Scenario: Player cannot open refrigerator menu while already holding one
    Given the player is holding an ingredient
    And the player is adjacent to the refrigerator
    When the player interacts with the refrigerator
    Then nothing happens
    And the player continues to hold the original ingredient

  # ─────────────────────────────────────────
  # TABLE (CHOPPING)
  # ─────────────────────────────────────────

  Scenario: Player chops a vegetable on the table
    Given the player is holding a raw vegetable
    And the table is unoccupied
    When the player interacts with the table
    Then the vegetable is placed on the table
    And a 2-second chopping timer begins
    And a UI progress indicator shows time remaining

  Scenario: Vegetable becomes chopped after 2 seconds
    Given a vegetable is being chopped on the table
    When 2 seconds have elapsed
    Then the vegetable is marked as chopped
    And its visual appearance changes to indicate it is chopped
    And the player can pick it up from the table

  Scenario: Table rejects a second vegetable while occupied
    Given a vegetable is already on the table
    And the player is holding a raw vegetable
    When the player interacts with the table
    Then nothing happens
    And the player continues holding the vegetable

  Scenario: Only a vegetable can be chopped on the table
    Given the player is holding a non-vegetable ingredient
    When the player interacts with the table
    Then nothing happens

  # ─────────────────────────────────────────
  # STOVE (COOKING)
  # ─────────────────────────────────────────

  Scenario: Player places meat on an empty stove slot
    Given the player is holding raw meat
    And at least one stove slot is empty
    When the player interacts with the stove
    Then the meat is placed on an available slot
    And a 6-second cooking timer begins for that slot
    And a UI progress indicator shows time remaining on that slot

  Scenario: Meat becomes cooked after 6 seconds
    Given raw meat is cooking on a stove slot
    When 6 seconds have elapsed
    Then the meat on that slot is marked as cooked
    And its visual appearance changes to indicate it is cooked
    And the player can pick it up from the stove

  Scenario: Player does not need to stay near the stove while meat cooks
    Given raw meat is placed on the stove
    When the player moves away from the stove
    Then the cooking timer continues running

  Scenario: Stove can cook two pieces of meat simultaneously
    Given both stove slots are occupied with raw meat
    Then each slot has an independent cooking timer running

  Scenario: Stove rejects meat when both slots are full
    Given both stove slots are occupied
    And the player is holding raw meat
    When the player interacts with the stove
    Then nothing happens
    And the player continues holding the raw meat

  Scenario: Only meat can be placed on the stove
    Given the player is holding a non-meat ingredient
    When the player interacts with the stove
    Then nothing happens

  # ─────────────────────────────────────────
  # INGREDIENT DELIVERY TO CUSTOMER WINDOW
  # ─────────────────────────────────────────

  Scenario: Player delivers a correct prepared ingredient to a window
    Given a customer window has an open order requiring a chopped vegetable
    And the player is holding a chopped vegetable
    And the player is adjacent to that window
    When the player interacts with the window
    Then the vegetable is removed from the player's hand
    And the order at that window records the vegetable as fulfilled

  Scenario: Player delivers cheese directly without preparation
    Given a customer window has an open order requiring cheese
    And the player is holding raw cheese
    And the player is adjacent to that window
    When the player interacts with the window
    Then the cheese is removed from the player's hand
    And the order at that window records the cheese as fulfilled

  Scenario: Player cannot deliver an unprepared ingredient that requires preparation
    Given a customer window has an open order requiring chopped vegetable
    And the player is holding a raw (unchopped) vegetable
    When the player interacts with the window
    Then the ingredient remains in the player's hand
    And the order is unchanged

  Scenario: Player cannot deliver an ingredient not required by the current order
    Given a customer window has an open order requiring only cheese
    And the player is holding cooked meat
    When the player interacts with the window
    Then the ingredient remains in the player's hand
    And the order is unchanged

  Scenario: Player cannot deliver an ingredient to a window with no active order
    Given a customer window has no active order
    And the player is holding an ingredient
    When the player interacts with the window
    Then the ingredient remains in the player's hand

  # ─────────────────────────────────────────
  # ORDER COMPLETION & SCORING
  # ─────────────────────────────────────────

  Scenario: Order completes when all required ingredients are delivered
    Given a customer window has an order requiring cheese and cooked meat
    And both cheese and cooked meat have been delivered to that window
    Then the order is marked as complete
    And the score for the order is calculated as ingredient values minus elapsed seconds (floored)
    And the score delta is displayed near the window (e.g. "+26")
    And the score delta fades away after a few seconds
    And the player's total score is updated

  Scenario: Order score can be negative if delivery takes too long
    Given a customer window has an order requiring only cheese (value 10)
    And the order has been active for 15 seconds when completed
    Then the score delta displayed is "-5"
    And the player's total score is decreased by 5

  Scenario: Score delta uses floored elapsed seconds
    Given an order worth 40 ingredient points completes after 14.99 seconds
    Then the time penalty applied is 14 points
    And the score delta displayed is "+26"

  Scenario: New order spawns 5 seconds after a window becomes empty
    Given a customer window has just had its order completed
    When 5 seconds elapse
    Then a new random order appears at that window

  # ─────────────────────────────────────────
  # ORDER GENERATION
  # ─────────────────────────────────────────

  Scenario: Orders contain either 2 or 3 ingredients with equal probability
    Given many orders are generated over time
    Then approximately 50% have 2 ingredients
    And approximately 50% have 3 ingredients

  Scenario: Order ingredients are chosen randomly including duplicates
    Given orders are generated over time
    Then any combination of vegetables, cheese, and meat is possible
    And duplicate ingredients within a single order are possible (e.g. meat, meat, meat)

  # ─────────────────────────────────────────
  # CUSTOMER WINDOW UI
  # ─────────────────────────────────────────

  Scenario: Customer window displays required ingredients for its order
    Given a customer window has an active order
    Then the window displays a visual indicator of each required ingredient
    And the indicators update as ingredients are delivered

  Scenario: Customer window displays elapsed time for active order
    Given a customer window has an active order
    Then a timer is visible at that window showing how long the order has been open
    And the timer counts up in real time

  # ─────────────────────────────────────────
  # TRASH
  # ─────────────────────────────────────────

  Scenario: Player discards a held ingredient at the trash
    Given the player is holding any ingredient (raw or prepared)
    And the player is adjacent to the trash
    When the player interacts with the trash
    Then the ingredient is removed from the player's hand
    And the player is now holding nothing

  # ─────────────────────────────────────────
  # GAME TIMER & END STATE
  # ─────────────────────────────────────────

  Scenario: Game ends when the 3-minute timer expires
    Given the game is running
    When the 3-minute countdown reaches zero
    Then the game ends
    And player input is disabled
    And a "Play Again" button is displayed

  Scenario: Game resets when Play Again is pressed
    Given the game over screen is displayed
    When the player clicks Play Again
    Then the score resets to zero
    And all orders are cleared and regenerated
    And the 3-minute timer resets
    And the game begins again

  Scenario: New high score is acknowledged at game over
    Given the player's final score exceeds the stored high score
    When the game ends
    Then a "New High Score!" message is displayed
    And the new high score is saved and persists to the next session

  Scenario: High score persists between game sessions
    Given the player achieved a high score of 200 in a previous session
    When a new game session is launched
    Then the displayed high score is 200

  # ─────────────────────────────────────────
  # HUD & OTHER UI
  # ─────────────────────────────────────────

  Scenario: HUD displays current score and high score during gameplay
    Given the game is running
    Then the current score is visible on screen
    And the all-time high score is visible on screen
    And both values update in real time

  Scenario: HUD displays the game countdown timer
    Given the game is running
    Then the remaining game time is visible on screen
    And it counts down in real time

  Scenario: Player can pause the game
    Given the game is running
    When the player clicks the Pause button
    Then the game timer stops
    And all cooking and chopping timers stop
    And all order timers stop

  Scenario: Player can resume after pausing
    Given the game is paused
    When the player clicks the Resume button
    Then all timers resume from where they were paused

  Scenario: Player can quit the game
    Given the game is running or on the start screen
    When the player clicks the Quit button
    Then the application exits
