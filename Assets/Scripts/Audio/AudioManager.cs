using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Creamos una instancia pública y la encapsulamos para poder usar sus métodos pero dejamos el private set
    //para conservar la integridad de la clase.
    public static AudioManager Instance {get; private set;}
    //Ponemos los componentes que necesitamos, en este caso 2 AudioSource que serán para la música y los efectos.
    [SerializeField] AudioSource music, soundEffects;

    //En el Awake() verificamos que solamente vaya a existir una instancia de esta clase durante la ejecución.
    private void Awake() {

        //Preguntamos si la Instance es diferente de null (ya existe).
        if(Instance != null)
        {
            //Si es así, destruimos este GameObject con todo dentro ya que sería un clon y/o repetición del script en la ejecución.
            Destroy(this.gameObject);
        }
        //Sino, es decir, si aún no hay una Instance de la clase, por defecto sería este script ya que cargó de primero.
        else
        {
            //Definimos que la instancia es este script.
            Instance = this;
            //Agregamos el GameObject al espacio de Dont Destroy On Load. Esto permite su permanencia a traves de todas las escenas.
            //En otras palabras no se elimina cuando cambiamos de escena.
            DontDestroyOnLoad(this.gameObject);
        }
    }

    //Creamos los métodos públicos que queremos utilizar en otros script SIN necesidad de hacer referencia a este script, sino, solamente
    //usando su Instance. Aquí tenemos un ejemplo de un método que reproducirá un clip de audio cada vez que ejecutemos el método PlaySFX()
    public void PlaySFX(AudioClip clip)
    {
        soundEffects.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip clip)
    {
        music.PlayDelayed(1);
    }

    //Para utilizar este método en otro Script, simplemente tendríamos que escribir en dicho script:
    //AudioManager.Instance.PlaySFX(clipDeAudio). Ese "clipDeAudio", seria un AudioClip referenciado específicamente para ese script.

}
