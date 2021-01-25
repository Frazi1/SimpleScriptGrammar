# Docs

```
<script> ::= <statement-list>

<statement-list> ::= (<statement> *)

<statement> :: = <block-statement> |
                 <if-statement> |
                 <while-statement> |
                 <assignment-statement>
 
 <block-statement> ::= "{" <statement-list> "}"
 <if-statement> ::= "if" "(" <expression> ")" <statement> ("else" <statement>)?
 <while-statement> ::= "while" "(" <expression> ")" <statement>
 <assignment-statement> :== <id> "=" <expression>
 <id> ::= <letter> (<letter> | <digit> )*
 
 <expression> ::= <logical-and-expression>
 <logical-and-expression> ::= <logical-or-expression> ("&" <logical-or-expression>)?
 <logical-or-expression> ::= <multiply-expression> ("|" <multiply-expression>)?
 <multiply-expression> ::= <sum-expression> ([*/] <sum-expression>)?
 <sum-expression> ::= <term> ([+-] <term>)?
 <term> ::= <digit> | <id> |  <group-expression> 
 <group-expression> ::= "(" <expression> ")"
 
 <letter> ::= [a-zA-Z]
 <digit> ::= [0-9]
 

```