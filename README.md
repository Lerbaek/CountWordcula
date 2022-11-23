# Create a word counter .NET program
## Requirements:
* Read data from multiple files in a given directory: directory path is input to command
line.
* Count the number of occurrences for each word from all files. (Create up to 4 files with
text of min 200 words - example form https://www.lipsum.com/) as input file.
* Words are case insensitive.
* Exclude any words from a list that is in exlude.txt file (create such a file and add 10
words that should be excluded from the output files.)
* Count the number of excluded words encountered, and save the result in a file.
* Create a file for each letter in the alphabet. Each file should include the words that start
with the letter and the count of occurrences:  
&nbsp;&nbsp;&nbsp;&nbsp;Sample output file:  
&nbsp;&nbsp;&nbsp;&nbsp;For letter L -> FILE_L.txt with content:  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;LORUM 10  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;LIKE 3  
&nbsp;&nbsp;&nbsp;&nbsp;For letter I -> FILE_I.txt content:  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;IPSUM 20  
&nbsp;&nbsp;&nbsp;&nbsp;FOR letter C - > FILE_C.txt content:  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;CONTENT 7  
And so on...

The task can be solved in many different ways. Please think about how well your code will perform
when running in an environment with multi core CPUs. Also consider how big a memory footprint
your solution will have.