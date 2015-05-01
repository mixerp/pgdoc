#PgDoc PostgreSQL Documenter
---
PgDoc creates beautiful database documentations from your PostgreSQL database. It is a small console application that runs
on:

* Windows
* Mac OSX
* Linux

![PgDoc Screenshot](https://raw.githubusercontent.com/mixerp/pgdoc/master/assets/images/pg-doc.png)

##Sample Documentation (Created by PgDoc)
[http://mixerp.org/erp/db-docs/](http://mixerp.org/erp/db-docs/)

---
#Compiled Binaries

##Windows Users
Windows users can download PgDoc from this url:

[http://mixerp.org/erp/mixerp-pgdoc.zip](http://mixerp.org/erp/mixerp-pgdoc.zip)

Note that the above link is achieved dependencies-packed, self-contained executable file. But before you try PgDoc, please make
sure that you have .net Framework 4.5 installed.

##OSX & Linux Users

Before you download PgDoc, you must have Mono framework installed. Depending upon your operating system, please download
and install Mono:

[http://www.mono-project.com/download/](http://www.mono-project.com/download/)

Link to PgDoc for Mono:

[http://mixerp.org/erp/mixerp-pgdoc-mono.zip](http://mixerp.org/erp/mixerp-pgdoc-mono.zip)

---

#Documentation
##Basic Syntax

**Windows**
```
MixERP.Net.Utilities.PgDoc.exe -s=[server[:port]] -d=[database] -u=[pg_user] -p=[pwd] -o=[output_dir]
```

Example:
```
MixERP.Net.Utilities.PgDoc.exe -s=localhost:17000 -d=mixerp -u=postgres -p=secret -o="c:\mixerp-doc"
```

**OSX & Linux**

```
mono /path/to/MixERP.Net.Utilities.PgDoc.exe -s=[server] -d=[database] -u=[pg_user] -p=[pwd] -o=[output_dir]
```

Example

```
mono /users/nirvan/desktop/pg-doc/MixERP.Net.Utilities.PgDoc.exe -s=localhost -d=mixerp -u=postgres -p=secret -o=/users/nirvan/desktop/db-doc
```

##Additional parameters
**-is: include schema**

Use a postgres regular expression to decide, which schemas should be documented. 
If you are not familiar with regular expressions, just create a list, using pipe (``|``) as separator.
```
MixERP.Net.Utilities.PgDoc.exe ... -is=schema1|schema2
```

**-xs: exclude schema**

Use a postgres regular expression to decide, which schemas should **not** be documented. You can use ``-is`` and ``-xs`` in combination
```
MixERP.Net.Utilities.PgDoc.exe ... -xs=tmp_.*
```
