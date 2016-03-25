# Upload large files to MVC / WebAPI using partitioning

Sample code for article:

Upload large files to MVC / WebAPI using partitioning

Published at: http://www.codeproject.com/Articles/1034347/Upload-large-files-to-MVC-WebAPI-using-partitionin

Sending large files to an MVC/Web-API server can be problematic - this article is about an alternative. The approach used is to break a large file up into small chunks, upload them, then merge them back together on the server - file transfer by partitioning. The article shows sending files to an MVC server from both a webpage using JavaScript, and a Web-form httpclient, and can be implemented using either MVC or WebAPI.
