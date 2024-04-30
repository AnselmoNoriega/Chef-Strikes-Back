INCLUDE globals.ink
VAR ingredientInHand = true

=== HowToThrow ===

{controller:
    {ingredientInHand: Alright, now throw that in the oven. Hold and release [RT]. How is it already cut? Through a complex process of stop-talking-and-do-what-I-tell you. | Here’s a hint: to make food, you need ingredients. Alright, now throw that in the oven. Hold and release [RT]. How is it already cut? Through a complex process of stop-talking-and-do-what-I-tell you.}
  - else:
    {ingredientInHand: Alright, now throw that in the oven. Hold and release [RMB]. How is it already cut? Through a complex process of stop-talking-and-do-what-I-tell you. | Here’s a hint: to make food, you need ingredients. Alright, now throw that in the oven. Hold and release [RMB]. How is it already cut? Through a complex process of stop-talking-and-do-what-I-tell you.}
}

-> END

