﻿use mysql;
ALTER USER 'root'@'%' IDENTIFIED WITH mysql_native_password BY '123456';

create database IF NOT EXISTS blog CHARSET utf8;

USE blog;

CREATE TABLE  IF NOT EXISTS `sys_user_info`(
  id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
  username VARCHAR(20) NOT NULL COMMENT '用户名称',
  `password` CHAR(32)  NOT NULL COMMENT '密码',
  role TINYINT(1) NOT NULL DEFAULT 0 COMMENT '用户角色 0普通用户 1系统管理员',
  `status`   TINYINT(1) NOT NULL DEFAULT 1 COMMENT '用户状态 1 启用  0停用',
   last_login_time DATETIME  NULL DEFAULT CURRENT_TIMESTAMP COMMENT '上次登录时间',
   is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
   createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
   modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间', 
   PRIMARY KEY(id)   
)ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '用户表';



CREATE TABLE  IF NOT EXISTS tbl_tag_info(
id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
tag_name VARCHAR(20) NOT NULL COMMENT '标签内容',
description VARCHAR(50) null COMMENT '描述',
  is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
   createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
   modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间', 
  PRIMARY KEY(id)
)ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '标签表';


CREATE TABLE  IF NOT EXISTS tbl_category_info(
id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
category_name VARCHAR(20) NOT NULL COMMENT '分类名称',
description VARCHAR(50) null COMMENT '描述',
is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',  
  PRIMARY KEY(id)
)ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '分类表';



CREATE TABLE  IF NOT EXISTS tbl_article_info (
 id INT(11) UNSIGNED AUTO_INCREMENT COMMENT '文章id',
 title VARCHAR(200) NOT NULL COMMENT '文章标题',
 abstract VARCHAR(500) NOT NULL COMMENT '文章摘要',
traffic INT  UNSIGNED NOT NULL DEFAULT 0 COMMENT '访问量',
 is_original  TINYINT(1) NOT NULL  DEFAULT 1  COMMENT '是否原创  1 是  0  不是',
  is_top  TINYINT(1) NOT NULL  DEFAULT 0  COMMENT '是否置顶  1 是  0  不是',
is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY(id)
) ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '文章表';


CREATE TABLE  IF NOT EXISTS tbl_article_content (

id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
article_id  INT(11) UNSIGNED COMMENT '文章id',
is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
content   TEXT NOT NULL COMMENT '文章内容',
createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
   PRIMARY KEY(id)
) ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '文章内容表';


CREATE TABLE  IF NOT EXISTS tbl_article_image (

id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
article_id  INT(11) UNSIGNED COMMENT '文章id',
image_url VARCHAR(100) NOT NULL COMMENT '图片链接',
is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
   PRIMARY KEY(id)
) ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '文章图片表';



CREATE TABLE  IF NOT EXISTS tbl_article_tag (

id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
article_id  INT(11) UNSIGNED COMMENT '文章id',
tag_id INT(11) NOT NULL COMMENT '标签id',
is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
   PRIMARY KEY(id)
) ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '文章标签表';


CREATE TABLE  IF NOT EXISTS tbl_article_category (

id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
article_id  INT(11) UNSIGNED COMMENT '文章id',
category_id INT(11) NOT NULL COMMENT '分类id',
is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
   PRIMARY KEY(id)
) ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '文章分类表';




CREATE TABLE  IF NOT EXISTS tbl_article_comment (

id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
article_id  INT(11) UNSIGNED COMMENT '文章id',
comment_id INT(11) NOT NULL COMMENT '评论id',
is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
   PRIMARY KEY(id)
) ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '文章评论表';




CREATE TABLE  IF NOT EXISTS tbl_comment (

id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
parent_id  INT(11) UNSIGNED NULL COMMENT '父id',
content VARCHAR(500) NOT NULL  COMMENT '留言内容',
create_by  INT(11) NOT NULL COMMENT '用户id',
is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
   PRIMARY KEY(id)
) ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '评论表';



CREATE TABLE  IF NOT EXISTS sys_view (

id INT(11) UNSIGNED AUTO_INCREMENT COMMENT 'id',
ip CHAR(15) NOT NULL COMMENT '访问ip',
is_deleted TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除 0 未删除 1 删除',
createtime DATETIME NOT NULL  DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
modifytime DATETIME  NULL   ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
   PRIMARY KEY(id)
) ENGINE=INNODB DEFAULT CHARSET=utf8 COMMENT '文章评论表';



CREATE VIEW IF NOT EXISTS  `blog`.`V_Article_Info` AS SELECT id,title,abstract,createtime,content,is_deleted FROM tbl_article_info AS a INNER JOIN tbl_article_content c ON a.id=c.article_id AND c.is_deleted="0" WHERE a.is_deleted="0"; 