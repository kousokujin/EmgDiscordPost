FROM postgres:9.3
ARG key="/var/lib/postgresql/data/server.key"
ARG crt="/var/lib/postgresql/data/server.crt"
ARG csr="/var/lib/postgresql/data/server.csr"
ARG all="/var/lib/postgresql/data/server.*"

RUN localedef -i ja_JP -c -f UTF-8 -A /usr/share/locale/locale.alias ja_JP.UTF-8
ENV LANG ja_JP.UTF-8

RUN openssl genrsa 2048 > server.key
#RUN openssl req -new -key $key > $crt
RUN openssl req -new -subj '/C=JP/ST=Tokyo/L=hogehoge/O=UNK/CN=test.local' -key server.key > server.csr 
RUN openssl x509 -days 36500 -req -signkey server.key < server.csr > server.crt
RUN chown postgres:postgres server.*
RUN chmod 600 server.key
RUN mv server.* /var/lib/postgresql/data/
run echo "ssl=on" >> /var/lib/postgresql/data/postgresql.conf
RUN echo "ssl_cert_file='server.crt'" >> /var/lib/postgresql/data/postgresql.conf
RUN echo "ssl_key_file='server.key'" >> /var/lib/postgresql/data/postgresql.conf

COPY *.sql /docker-entrypoint-initdb.d/


EXPOSE 5432
EXPOSE 443
