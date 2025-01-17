﻿Markdown uses email\-style \> characters for blockquoting\. If you’re familiar with quoting passages of text in an email message, then you know how to create a blockquote in Markdown\. It looks best if you hard wrap the text and put a \> before every line\:

>	This is a blockquote with two paragraphs\. Lorem ipsum dolor sit amet, consectetuer adipiscing elit\. Aliquam hendrerit mi posuere lectus\. Vestibulum enim wisi, viverra nec, fringilla in, laoreet vitae, risus\.
>	
>	Donec sit amet nisl\. Aliquam semper ipsum sit amet velit\. Suspendisse id sem consectetuer libero luctus adipiscing\.
>	

Markdown allows you to be lazy and only put the \> before the first line of a hard\-wrapped paragraph\:

>	This is a blockquote with two paragraphs\. Lorem ipsum dolor sit amet, consectetuer adipiscing elit\. Aliquam hendrerit mi posuere lectus\. Vestibulum enim wisi, viverra nec, fringilla in, laoreet vitae, risus\.
>	
>	Donec sit amet nisl\. Aliquam semper ipsum sit amet velit\. Suspendisse id sem consectetuer libero luctus adipiscing\.
>	

Blockquotes can be nested \(i\.e\. a blockquote\-in\-a\-blockquote\) by adding additional levels of \>\:

>	This is the first level of quoting\.
>	
>	>	This is nested blockquote\.
>	>	
>	
>	Back to the first level\.
>	

Blockquotes can contain other Markdown elements, including headers, lists, and code blocks\:

>	## This is a header\.
>	
>	#.	This is the first list item\.
>	
>	#.	This is the second list item\.
>	
>	
>	Here’s some example code\:
>	
>	```
>	return shell_exec("echo $input | $markdown_script");
>	```
>	

Blockquotes can also be used do illustrate editing\:

+>	This paragraph has been inserted
+>	

->	While this paragraph has been deleted
->	

