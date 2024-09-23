EXTERNAL causeEvent(ID)
-> main

=== main ===
You stumble upon two paths. One overflowing with riches. One treacherous and most unkind.
Which path do you go down?
    + [Treacherous]
        ->Treacherous
    + [Rich]
        ->Rich
    
=== Treacherous ===
you are robbed of one of your resources. Which do you lose?
    + [food]
        ~causeEvent(4)
        ->give("food")
    + [money]
        ~causeEvent(8)
        ->give("money")
    + [fuel]
        ~causeEvent(3)
        ->give("fuel")

===Rich ===
you stumble upon three piles of resources. Which do you choose?
+ [food]
        ~causeEvent(6)
        ->gain("food")
        
    + [money]
        ~causeEvent(7)
        ->gain("money")

    + [fuel]
        ~causeEvent(5)
        ->gain("fuel")


=== give(res)===
you gave up {res}
->END
=== gain(res) ===
you gain {res}

->END


