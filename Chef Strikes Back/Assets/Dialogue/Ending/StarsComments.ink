VAR stars = 0

->PlayerDeath
==PlayerDeath
{stars == 1:
    Alright, well, I’ve seen better first tries. No I’m being too nice, it’s the worst one I’ve ever seen. However many stars you get is how much extra money you get. If you want to move on to the next level, you need [#MONEY]. So you better try again is what I’m saying.
}

{stars == 2:
   You’re one of these “your best is enough” chumps aren’t you? Sigh. Well, here’s hoping you get better. However many stars you get is how much extra money you get. If you want to move on to the next level, you need [#MONEY]. So you better try again is what I’m saying.
}

{stars == 3 || 4:
Hm, good enough. There might be potential in you yet. However many stars you get is how much extra money you get. If you want to move on to the next level, you need [#MONEY]. You should have enough to move on if you need to. Or, you can try again, if you’re one of those perfectionist types.
}

{stars == 5:
Wow… Alright… good enough.
Could he be… The One?
Anyway… However many stars you get is how much extra money you get. If you want to move on to the next level, you need [#MONEY]. So you’re good to move on is what I’m saying.

}

->DONE