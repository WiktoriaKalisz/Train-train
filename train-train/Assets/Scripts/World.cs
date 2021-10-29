using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor.Animations;

public class Level {
    private List<SymbolMapping> symbols;
    private List<Texture2D> passengers;
    private Train train;
    private bool doesEnd;
    private bool limitPassengers;
    private bool calmBackground;
    private bool leftHand;


    public Level(Station firstStation, bool doesEnd, bool limitPassengers, Train train, List<SymbolMapping> symbols, List<Texture2D> passengers, bool calmBackground, bool leftHand) {
        this.symbols = symbols;
        this.passengers = passengers;
        this.train = train;
        this.doesEnd = doesEnd;
        this.limitPassengers = limitPassengers;
        this.calmBackground = calmBackground;
        this.leftHand = leftHand;

        var toSpawn = train.seats.Count;
        foreach (var seat in firstStation.seats) {
            if (Random.value < (limitPassengers ? 0.9 : 0.4)) {
                if (limitPassengers && toSpawn-- <= 0) {
                    continue;
                }

                var p = getNextPassenger(firstStation);
                if (p != null) {
                    seat.Place(p);
                }
            }

        }
    }

    public SymbolMapping getNextStationSymbolMapping() {
        if (symbols.Count == 0) { return null; }

        var s = symbols[0];
        symbols.RemoveAt(0);
        if (!doesEnd) {
            symbols.Add(s);
        }

        return s;
    }

    public Symbol getRandomPossibleDestination(Station station) {
        if (symbols.Count == 0) { return null; }
        var destinations = symbols.GetRange(0, symbols.Count);
        var next_index = System.Math.Min(Random.Range(0, destinations.Count), Random.Range(0, destinations.Count));
        return destinations[next_index].randomMatching();
    }

    public Texture2D getRandomPassengerTexture() {
        return passengers[Random.Range(0, passengers.Count)];
    }

    public Passenger getNextPassenger(Station station) {
        var t = getRandomPassengerTexture();
        if (t == null) {
            return null;
        }

        var d = getRandomPossibleDestination(station);
        if (d == null) {
            return null;
        }

        return Passenger.GetPassenger(d, t);
    }

    public Station spawnNextStation() {
        var nextSymbolMapping = getNextStationSymbolMapping();
        if (nextSymbolMapping == null) {
            return null;
        }

        var newstation = Station.Spawn(nextSymbolMapping, Random.Range(40, 80));

        var toSpawn = train.seats.Count(s => s.isEmpty() || newstation.doesMatch(s.passenger));

        foreach (var seat in newstation.seats) {
            if (Random.value < (Data.Profile.limitPassengers ? 0.9 : 0.4)) {
                if (Data.Profile.limitPassengers && toSpawn-- <= 0) {
                    continue;
                }

                var p = getNextPassenger(newstation);
                if (p != null) {
                    seat.Place(p);
                }
            }

        }

        return newstation;
    }

}

public class World : MonoBehaviour {

    public Level level;
    public Station station;
    public Train train;
    public Environment environment;
    public Text scoreText;
    public Text back;
    public Text startText;
    public Text endText;
    public Text pointsText;
    public Text chooseText;
    private int score = 0;
    private bool quit = false;
    private float _newStationDistance = 100;
    private bool enablePassengerMove = false;
    public Vector2 StationCenterPosition;
    public GameObject trainObject;
    public GameObject directionalLightObject;
    public Light directionalLight;
    private bool alreadyStarted = false;


    private void Start() {
        Input.multiTouchEnabled = false;
        var passengers = Data.Profile.passengers.selected();
        var symbols = Data.Profile.Symbols;
        level = new Level(station, Data.Profile.doesEnd, Data.Profile.limitPassengers, train, symbols, passengers, Data.Profile.calmBackground, Data.Profile.leftHand);
        train.SpeedLimit = Data.Profile.trainSpeed;
        train.driver = Data.Profile.drivers.selected();
        scoreText.gameObject.SetActive(Data.Profile.allowScore);
        pointsText.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        endText.gameObject.SetActive(false);
        train.move.gameObject.SetActive(Data.Profile.allowLabels && Data.Profile.leftHand);
        train.move2.gameObject.SetActive(Data.Profile.allowLabels && !Data.Profile.leftHand);
        chooseText.gameObject.SetActive(false);

        ShowStart();
        HideBack();
        HideMoves();
        ShowChoose();

        GameObject.Find("background_2_1422x768@2x").SetActive(!Data.Profile.calmBackground);
        GameObject.Find("background_1_1422x768@2x").SetActive(!Data.Profile.calmBackground);
        GameObject.Find("calm_background_1@2x").SetActive(Data.Profile.calmBackground);
        GameObject.Find("calm_background_2@2x").SetActive(Data.Profile.calmBackground);
        train.arrow.SetActive(!Data.Profile.leftHand);
        train.arrow2.SetActive(Data.Profile.leftHand);
        directionalLightObject = GameObject.Find("directionalLightObject");
        directionalLight = directionalLightObject.GetComponent<Light>();
        /*
        if (Data.Profile.contrast >= 0 && Data.Profile.contrast < 10)
        {
            directionalLight.color = Color.red;
            // directionalLight.color = new Color(Data.Profile.contrast, 0, 0);
        }
        else if (Data.Profile.contrast >= 10 && Data.Profile.contrast < 20)
        {
            directionalLight.color = Color.green;
            //directionalLight.color = new Color(0, Data.Profile.contrast, 0);
        }
        else
        {
            directionalLight.color = Color.blue;
            //directionalLight.color = new Color(0,0, Data.Profile.contrast);
        }
        */
        switch (Data.Profile.colorScheme)
        {
            case 0:
                break;
            case 1:
                train.c_renderer.SetColor(Color.yellow);
                directionalLight.color = Color.blue;
                break;
            case 2:
                train.c_renderer.SetColor(Color.cyan);
                directionalLight.color = Color.yellow;
                break;
            case 3:
                train.c_renderer.SetColor(Color.yellow);
                directionalLight.color = Color.cyan;
                break;
            default:
                break;
        }

        //directionalLight.color = Color.red;

        // directionalLight = directionalLightObject.GetComponent<Light>();
        //directionalLight.color = Color.red;

        if (Data.Profile.leftHand)
        {
            Animator otherAnimator;
            trainObject = GameObject.Find("Train");
            otherAnimator = trainObject.GetComponent<Animator>();
            otherAnimator.Play("arrow222");
        }
        //AnimatorStateMachine asm = cont.layers[0].stateMachine;
        //AnimatorState newState = asm.AddState("Default State");
        //asm.defaultState = newState;

        // AnimatorStateMachine asm = otherAnimator.layers[0].stateMachine;
        // asm.defaultState = 
    }

    private void Update() {
        SpawnStations();
        HandleInput();
        MoveWorld();
        ShowEnd();
        train.Decelerate();
        if (quit) {
            quitGame();
        }
        if (alreadyStarted)
        {
            train.move.gameObject.SetActive(false);
            train.move2.gameObject.SetActive(false);
        }

            var rect = station.GetComponent<BoxCollider2D>().bounds;
        enablePassengerMove = train.seats.All(seat => rect.Contains(seat.transform.position));


        if(Data.Profile.left)
            chooseText.gameObject.SetActive(false);





        foreach (Seat seat in FindObjectsOfType<Seat>()) {
           
            seat.setActive(train.Speed == 0 && enablePassengerMove);
            
        }

        var passengersToLeave = station.seats.FindAll(s => (s.passenger && station.doesMatch(s.passenger))); 
        foreach (var seat in passengersToLeave) {
            seat.leaveSeat();
            score += 3;
            scoreText.text = score.ToString();
        }

    }

    private void MoveWorld() {
        environment.SetMoveSpeed(-train.Speed);
        foreach (GameObject station in GameObject.FindGameObjectsWithTag("Station")) {
            station.transform.Translate(Time.deltaTime * -train.Speed, 0.0f, 0.0f);
        }
    }

    public void quitGame() {
        StartCoroutine(removeTrain());
    }

    public void ShowStart() {
        StartCoroutine(showText());
    }

    public void ShowEnd()
    {
        StartCoroutine(showTextEnding());
    }

    public void HideBack()
    {
        StartCoroutine(removeBack());
    }

    public void HideMoves()
    {
        StartCoroutine(removeMoves());
    }

    public void ShowChoose()
    {
        StartCoroutine(showChooseTag());
    }

   

    private IEnumerator showChooseTag()
    {
        if (Data.Profile.allowLabels)
        {
            yield return new WaitForSeconds(6);
            if (Data.Profile.left == false)
            {
                    chooseText.gameObject.SetActive(true);
            }
        }        
    }

   

    private IEnumerator removeTrain() {
        train.playLeave();
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator showText() {
        if (Data.Profile.allowLabels)
        {
            yield return new WaitForSeconds(4);
            if(!alreadyStarted)
            startText.gameObject.SetActive(true);
        }
    }

    private IEnumerator showTextEnding()
    {
        if (Data.Profile.end == true)
        {
            pointsText.text = score.ToString();
            yield return new WaitForSeconds(1);           
            endText.gameObject.SetActive(true);
            pointsText.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            endText.gameObject.SetActive(false);
            pointsText.gameObject.SetActive(false);
            Data.Profile.end = false;
            SceneManager.LoadScene("Menu");
            Data.Profile.end = false;
        }
    }

    private IEnumerator removeBack()
    {
        if (Data.Profile.allowLabels)
        {
            yield return new WaitForSeconds(4);
            back.gameObject.SetActive(false);
        }
    }

    private IEnumerator removeMoves()
    {
        if (Data.Profile.allowLabels && Data.Profile.leftHand)
        {
            yield return new WaitForSeconds(4);
            train.move.gameObject.SetActive(false);
        }

        if (Data.Profile.allowLabels && !Data.Profile.leftHand)
        {
            yield return new WaitForSeconds(4);
            train.move2.gameObject.SetActive(false);
        }
    }


    private void SpawnStations() {
        var trainx = train.transform.position.x;
        var stationx = station.transform.position.x;
        if (trainx - stationx < 30) {
            return;
        }

        train.seats.Select(s => s.passenger).ToList().FindAll(p => p != null && station.doesMatch(p)).ForEach(s => { s.playSad(); score -= 3;
            scoreText.text = score.ToString();
        });
       


        var newStation = level.spawnNextStation();
        if (newStation == null) {
            quit = true;
            return;
        }

        Destroy(station.gameObject);
        station = newStation;
    }

    private void stopOnStation(Train train, Station station) {
        if (train.Speed != 0) {
            train.arrow.SetActive(false);
            train.Stop();
        }
    }

    private void reseatToStation(Seat seat, Station station) {
        var passenger = seat.passenger;
        if (!passenger) { return; }
        var station_seat = station.FreeSeat();
        if (!station_seat) { return; }

        passenger.image.canvas.sortingOrder = 2000;
        station_seat.Place(seat.Remove());

        if (passenger.symbolRepresentation.symbol != station.symbolRepresentation.symbol) {
            passenger.playSad();
            scoreText.text = (--score).ToString();
        }
    }

    private void reseatToTrain(Seat seat, Train train) {
        var passenger = seat.passenger;
        if (!passenger) { return; }
        var train_seat = train.FreeSeat();
        if (!train_seat) { return; }

        passenger.image.canvas.sortingOrder = 19;
        train_seat.Place(seat.Remove());
    }

    private void handleHit(GameObject gameObject) {

        if (gameObject.CompareTag("Accelerate")) {
            train.Accelerate();
            train.arrow.SetActive(false);
            train.arrow2.SetActive(false);
            startText.gameObject.SetActive(false);
            alreadyStarted = true;
        }

        if (gameObject.CompareTag("Train Seat")) {
            var seat = gameObject.GetComponent<Seat>();
            reseatToStation(seat, station);
        }

        if (gameObject.CompareTag("Station Seat")) {
            var seat = gameObject.GetComponent<Seat>();
            reseatToTrain(seat, train);
        }
    }

    private void HandleInput() {
        Vector2? position = null;

       // train.arrow.SetActive(true);

        if (Input.touchCount > 0) {
            position = Input.touches[0].position;
        }

        if (Input.GetMouseButton(0)) {
            position = Input.mousePosition;
        }

        if (!position.HasValue) {
            stopOnStation(train, station);
            return;
        }

        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(position.Value), out hit, 500)) {
            train.Break();
            return;
        }

        var gameObject = hit.collider.gameObject;
        handleHit(gameObject);

    }

}
