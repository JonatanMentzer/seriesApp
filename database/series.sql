CREATE TABLE series (
    seriesid SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    releaseyear INT,
    company TEXT NOT NULL,
    rating INT CHECK (rating BETWEEN 1 AND 10)
);

CREATE TABLE genres (
    genreid SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE
);

CREATE TABLE creators (
    creatorid SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE
);

CREATE TABLE seriesgenres (
    seriesid INT REFERENCES series(seriesid) ON DELETE CASCADE,
    genreid INT REFERENCES genres(genreid) ON DELETE CASCADE,
    PRIMARY KEY (seriesid, genreid)
);

CREATE TABLE seriescreators (
    seriesid INT REFERENCES series(seriesid) ON DELETE CASCADE,
    creatorid INT REFERENCES creators(creatorid) ON DELETE CASCADE,
    PRIMARY KEY (seriesid, creatorid)
);
