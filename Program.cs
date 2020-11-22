﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamning_3_Ra_Kod_Alhussein
{
    /* CLASS: letter
    * PURPOSE: Make a class Letter with variables and a assembler to 
    *  lay up information about the letter and the value of it 
    */
    


    public class Letter
    {
        public string letters;
        public double value = 0;
        

        public Letter(string letters)
        {
            this.letters = letters;
        }
    }
    /* CLASS: CStack
     * PURPOSE: Is essentially a RPN-calculator with four registers X, Y, Z, T
     *   like the HP RPN calculators. Numeric values are entered in the entry
     *   string by adding digits and one comma. For test purposes the method
     *   RollSetX can be used instead. Operations can be performed on the
     *   calculator preferrably by using one of the methods
     *     1. BinOp - merges X and Y into X via an operation and rolls down
     *        the stack
     *     2. Unop - operates X and puts the result in X with overwrite
     *     3. Nilop - adds a known constant on the stack and rolls up the stack
     */ 
    public class CStack
    {
        public double X, Y, Z, T;
        public string entry;
        public string ChosenLetter; 

        public List<Letter> LetterValues = new List<Letter>();
        /* CONSTRUCTOR: CStack
         * PURPOSE: create a new stack and init X, Y, Z, T and the text entry
         * PARAMETERS: --
         */ 
        public CStack()
        {
            X = Y = Z = T = 0;
            entry = "";

            Letter A = new Letter("A");
            LetterValues.Add(A);
            Letter B = new Letter("B");
            LetterValues.Add(B);
            Letter C = new Letter("C");
            LetterValues.Add(C);
            Letter D = new Letter("D");
            LetterValues.Add(D);
            Letter E = new Letter("E");
            LetterValues.Add(E);
            Letter F = new Letter("F");
            LetterValues.Add(F);
            Letter G = new Letter("G");
            LetterValues.Add(G);
            Letter H = new Letter("H");
            LetterValues.Add(H);
        }
        /* METHOD: Exit
        * PURPOSE: called on exit, prepared for saving
        * PARAMETERS: --
        * RETURNS: --
        */ 

        public void Exit()
        {

        }
        /* METHOD: StackString
         * PURPOSE: construct a string to write out in a stack view
         * PARAMETERS: --
         * RETURNS: the string containing the values T, Z, Y, X with newlines 
         *   between them
         */ 
         public string StackString()
        {
            return $"{T}\n{Z}\n{Y}\n{X}\n{entry}";
        }
        /* METHOD: VarString
         * PURPOSE: construct a string to write out in a variable list
         * PARAMETERS: --
         * RETURNS: NOT YET IMPLEMENTED
         */ 

         public string VarString()
        {
            string ValuesList = "";
            for(int i=0; i < LetterValues.Count(); i++)
            {
                if(LetterValues[i].value != 0)
                {
                    ValuesList += LetterValues[i].value.ToString() + "\n";
                }
                else
                {
                    ValuesList += "\n";
                }
            }
            return $"{ValuesList}";
        }
        /* METHOD: SetX
       * PURPOSE: set X with overwrite
       * PARAMETERS: double newX - the new value to put in X
       * RETURNS: --
       */ 
        public void SetX(double newX)
        {
            X = newX;
        }
        /* METHOD: EntryAddNum
         * PURPOSE: add a digit to the entry string
         * PARAMETERS: string digit - the candidate digit to add at the end of the
         *   string
         * RETURNS: --
         * FAILS: if the string digit does not contain a parseable integer, nothing
         *   is added to the entry
         */ 
         public void EntryAddNum(String digit)
        {
            int val;
            if(int.TryParse(digit,out val))
            {
                entry = entry + val;
            }
        }
        /* METHOD: EntryAddComma
        * PURPOSE: adds a comma to the entry string
        * PARAMETERS: --
        * RETURNS: --
        * FAILS: if the entry string already contains a comma, nothing is added
        *   to the entry
        */ 
        public void EntryAddComa()
        {
            if (entry.IndexOf(",") == -1)
            {
                entry = entry + ",";
            }
        }
        /* METHOD: EntryChangeSign
        * PURPOSE: changes the sign of the entry string
        * PARAMETERS: --
        * RETURNS: --
        * FEATURES: if the first char is already a '-' it is exchanged for a '+',
        *   if it is a '+' it is changed to a '-', otherwise a '-' is just added
        *   first
        */ 
        public void EntryChangeSign()
        {
            char[] cval = entry.ToCharArray();
            if(cval.Length > 0)
            {
                switch (cval[0])
                {
                    case '+': cval[0] = '-'; entry = new string(cval); break;
                    case '-': cval[0] = '+'; entry = new string(cval); break;
                    default: entry = '-' + entry; break;
                }
            }
            else
            {
                entry= '-' + entry;
            }
        }
        /* METHOD: Enter
         * PURPOSE: converts the entry to a double and puts it into X
         * PARAMETERS: --
         * RETURNS: --
         * FEATURES: the entry is cleared after a successful operation
         */ 
        public void Enter()
        {
            if ( entry != "")
            {
                RollSetX(double.Parse(entry));
                entry = "";
            }
            
        }
        /* METHOD: Drop
        * PURPOSE: drops the value of X, and rolls down
        * PARAMETERS: --
        * RETURNS: --
        * FEATURES: Z gets the value of T
        */ 
        public void Drop()
        {
            X = Y; Y = Z; Z = T;
        }
        /* METHOD: DropSetX
         * PURPOSE: replaces the value of X, and rolls down
         * PARAMETERS: double newX - the new value to assign to X
         * RETURNS: --
         * FEATURES: Z gets the value of T
         * NOTES: this is used when applying binary operations consuming
         *   X and Y and putting the result in X, while rolling down the
         *   stack
         */ 
         
          public void DropSetX(double newX)
          {
            X = newX; Y = Z; Z = T;
          }
        /* METHOD: BinOp
       * PURPOSE: evaluates a binary operation
       * PARAMETERS: string op - the binary operation retrieved from the
       *   GUI buttons
       * RETURNS: --
       * FEATURES: the stack is rolled down
       */ 
        public void BinOp(string op)
        {
            switch (op)
            {
                case "+": DropSetX(Y + X); break;
                case "−": DropSetX(Y - X); break;
                case "×": DropSetX(Y * X); break;
                case "÷": DropSetX(Y / X); break;
                case "yˣ": DropSetX(Math.Pow(Y, X)); break;
                case "ˣ√y": DropSetX(Math.Pow(Y, 1.0 / X)); break;
            }
        }
        /* METHOD: Unop
         * PURPOSE: evaluates a unary operation
         * PARAMETERS: string op - the unary operation retrieved from the
         *   GUI buttons
         * RETURNS: --
         * FEATURES: the stack is not moved, X is replaced by the result of
         *   the operation
         */ 
        public void Unop(string op)
        {
            switch (op)
            {
                // Powers & Logarithms:
                case "x²": SetX(X * X); break;
                case "√x": SetX(Math.Sqrt(X)); break;
                case "log x": SetX(Math.Log10(X)); break;
                case "ln x": SetX(Math.Log(X)); break;
                case "10ˣ": SetX(Math.Pow(10, X)); break;
                case "eˣ": SetX(Math.Exp(X)); break;

                // Trigonometry:
                case "sin": SetX(Math.Sin(X)); break;
                case "cos": SetX(Math.Cos(X)); break;
                case "tan": SetX(Math.Tan(X)); break;
                case "sin⁻¹": SetX(Math.Asin(X)); break;
                case "cos⁻¹": SetX(Math.Acos(X)); break;
                case "tan⁻¹": SetX(Math.Atan(X)); break;
            }
        }
        /* METHOD: Nilop
       * PURPOSE: evaluates a "nilary operation" (insertion of a constant)
       * PARAMETERS: string op - the nilary operation (name of the constant)
       *   retrieved from the GUI buttons
       * RETURNS: --
       * FEATURES: the stack is rolled up, X is preserved in Y that is preserved in
       *   Z that is preserved in T, T is erased
       */ 
        public void Nilop(string op)
        {
            switch (op)
            {
                case "π": RollSetX(Math.PI); break;
                case "e": RollSetX(Math.E); break;
            }
        }
        /* METHOD: Roll
         * PURPOSE: rolls the stack up
         * PARAMETERS: --
         * RETURNS: --
         */ 
        public void Roll()
        {
            double tmp = T;
            T = Z; Z = Y; Y = X; X = tmp; 
        }
        /* METHOD: Roll
         * PURPOSE: rolls the stack up and puts a new value in X
         * PARAMETERS: double newX - the new value to put into X
         * RETURNS: --
         * FEATURES: T is dropped
         */ 
        public void RollSetX(double newX)
        {
            T = Z; Z = Y; Y = X; X = newX; 

        }
        /* METHOD: SetAddress
        * PURPOSE: 
        * PARAMETERS: string name - variable name
        * RETURNS: --
        * FEATURES: NOT YET IMPLEMENTED
        */ 
        public void SetAdress(string name)
        {
            switch (name)
            {
                case "A": ChosenLetter = name; break;
                case "B": ChosenLetter= name; break;
                case "C": ChosenLetter = name; break;
                case "D": ChosenLetter = name; break;
                case "E": ChosenLetter = name; break;
                case "F": ChosenLetter = name; break;
                case "G": ChosenLetter = name; break;
                case "H": ChosenLetter = name; break;
            }
        }
        /* METHOD: SetVar
        * PURPOSE: Looks for the same string value and adds the value of X to the list object
        * PARAMETERS: --
        * RETURNS: --
        * FEATURES: NOT YET IMPLEMENTED
        */ 
        public void SetVar()
        {
            for(int i=0; i < LetterValues.Count(); i++)
            {
                if (LetterValues[i].letters == ChosenLetter)
                {
                    LetterValues[i].value = X;
                }
            }
        }
        /* METHOD: GetVar
         * PURPOSE: Move the value from list object to x
         * PARAMETERS: --
         * RETURNS: --
         * FEATURES: NOT YET IMPLEMENTED
         */ 
         public void GetVar()
        {
            for(int i=0; i < LetterValues.Count(); i++)
            {
                if (LetterValues[i].letters == ChosenLetter)
                {
                    RollSetX(LetterValues[i].value);
                    LetterValues[i].value = 0;
                }
            }
        }









    }
}
