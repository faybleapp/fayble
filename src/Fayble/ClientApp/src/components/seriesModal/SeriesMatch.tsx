import ComicVine from "assets/logos/providers/ComicVine.png";
import GCD from "assets/logos/providers/GrandComicsDatabase.png";
import { Image } from "components/image";
import { LoadingIndicator } from "components/loadingIndicator";
import { Button, Container, Form, InputGroup } from "react-bootstrap";
import { useSeriesMetadata } from "services";
import styles from "./SeriesMatch.module.scss";

interface SeriesMatchProps {
  seriesId: string;
  unMatchSeries: () => void;
}

export const SeriesMatch = ({ seriesId, unMatchSeries }: SeriesMatchProps) => {
  const { data: matchedSeries, isLoading } = useSeriesMetadata(seriesId);

  return (
    <Container>
      {isLoading ? (
        <div className={styles.loadingIndicator}>
          <LoadingIndicator />
        </div>
      ) : (
        <div className={styles.container}>
          <div className={styles.coverContainer}>
            <Image
              className={styles.cover}
              src="https://comicvine.gamespot.com/a/uploads/original/12/124613/7450929-star%20wars%20omnibus%20-%20darth%20vader%20pagecover.jpg"></Image>
          </div>
          <div className={styles.detailsContainer}>
            <InputGroup className="mb-3">
              <InputGroup.Text            
                className={styles.providerInputLogo}>
                Fayble
              </InputGroup.Text>

              <Form.Control
                placeholder="4000-55260"
                aria-label="Username"
                aria-describedby="basic-addon1"
                value={matchedSeries?.id}
              />
            </InputGroup>
            <InputGroup className="mb-3">
              <InputGroup.Text
                id="basic-addon1"
                className={styles.providerInputLogo}>
                <img src={ComicVine} style={{ width: "43px" }} />
              </InputGroup.Text>

              <Form.Control
                placeholder="4000-55260"
                aria-label="Username"
                aria-describedby="basic-addon1"
              />
            </InputGroup>
            <InputGroup className="mb-3">
              <InputGroup.Text
                id="basic-addon1"
                className={styles.providerInputLogo}>
                <img
                  src={GCD}
                  style={{
                    width: "43px",
                    background: "#2f394b",
                    border: "none",
                  }}
                />
              </InputGroup.Text>

              <Form.Control
                placeholder="1700021"
                aria-label="Username"
                aria-describedby="basic-addon1"
              />
            </InputGroup>
            <Button className={styles.unmatchButton} variant="primary" size="sm" onClick={unMatchSeries}>
            Unmatch
          </Button>
          </div>
  
        </div>
      )}
    </Container>
  );
};
