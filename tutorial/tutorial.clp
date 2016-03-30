
;;;==============================================
;;;												*
;;; Custom file for the ComS 437 Tutorial Demo  *
;;;												*
;;;==============================================

(deftemplate player
	(field name))

(deftemplate monster 
	(field name) 
	(field type))

(deftemplate weapon
	(field name)
	(field type))
	
(defrule startup
	(initial-fact)
	=>
	(assert
		(monster (name Salamander) (type fire))
		(monster (name Tortoise) (type earth))
		(monster (name Eagle) (type wind))
		(monster (name Shark) (type water))
		(monster (name Thunder_Rat) (type electricity))
		(weapon (name Torch) (type fire))
		(weapon (name Giant_Rock) (type earth))
		(weapon (name Water_Balloon) (type water))
		(weapon (name Fan) (type wind))
		(weapon (name Stun_Gun) (type electricity)))

	(printout t "Please enter your name." crlf)
	(bind ?pID (read))
	
	(assert (player (name ?pID)))
	
	(printout t crlf "Hello " ?pID crlf)
	
	(printout t "Would you like to hunt monsters? (yes/no)" crlf)
	(bind ?answer (read))
	(printout t crlf crlf)
	
	(assert (answer ?answer))
	
	;(while (= (str-compare "yes" ?answer) 0)
	;	(printout t "Would you like to hunt again? (yes/no)" crlf)
	;	(bind ?answer (read))
	;	(printout t crlf crlf))
		
	;(printout t "Goodbye," ?pID "!" crlf crlf)
)

(defrule game-loop-success
	
		?aID <- (answer ?answer)
		(test (= (str-compare "yes" ?answer) 0))
	=>
		(retract ?aID)
		(printout t "Would you like to hunt again? (yes/no)" crlf)
		(bind ?answer (read))
		(printout t crlf crlf)	
		(assert (answer ?answer))
)

(defrule game-loop-fail
	
		?aID <- (answer ?answer)
		(test (neq (str-compare "yes" ?answer) 0))
	=>
		(retract ?aID)
		(printout t "Goodbye" crlf)
)
		