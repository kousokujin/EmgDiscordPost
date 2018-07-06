FROM centos:7

MAINTAINER kousokujin

ARG ver=1.0.0.2
ARG url=https://github.com/kousokujin/PSO2emergencyGetter/releases/download/${ver}/PSO2emergencyGetter_Linux_${ver}.zip
ENV TZ Asia/Tokyo
ENV LANG ja_JP.UTF-8
ENV LC_TIME ja_JP.UTF-8

RUN yum update -y && yum install -y wget unzip
RUN yum install -y libicu libunwind
RUN mkdir /var/pso2
RUN wget -P /var/pso2 ${url}
RUN unzip -d /var/pso2 /var/pso2/PSO2emergencyGetter_Linux_${ver}.zip
RUN mkdir /var/pso2/PSO2emergencyGetter_Linux_${ver}/config
RUN chmod 755 /var/pso2/PSO2emergencyGetter_Linux_${ver}/*

COPY config /var/pso2/PSO2emergencyGetter_Linux_${ver}/config
RUN chmod 755 /var/pso2/PSO2emergencyGetter_Linux_${ver}/config/conf.txt
#VOLUME /var/pso2/PSO2emergencyGetter_Linux_1.0.0.1/config
#CMD ["/bin/bash"]
#CMD ["sleep", "10;","/var/pso2/PSO2emergencyGetter_Linux_1.0.0.1/PSO2emergencyGetter","/var/pso2/PSO2emergencyGetter_Linux_1.0.0.1/config/conf.txt"]

COPY init.sh /init.sh
RUN chmod 744 /init.sh
CMD ["/init.sh"]
